using LukeApps.AlertHandling;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.Authorization;
using LukeApps.EmailHandling;
using LukeApps.EmployeeData;
using PhilApprovalFlow;
using PhilApprovalFlow.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace LukeApps.ApprovalProcess
{
    public static class ApprovalExtensions
    {
        public static string GetDisplay(this DecisionType value)
        {
            return value == DecisionType.AwaitingDecision ? "Awaiting Decision" : value.ToString();
        }

        public static ICanAction SetUserName(this ICanSetUser approvalFlow, Employee employee)
        {
            return approvalFlow.SetUserName(employee.Username);
        }

        public static ICanAction RequestApproval(this ICanAction approvalFlow, Employee employee)
        {
            return approvalFlow.RequestApproval(employee.Username);
        }

        public static ICanAction RequestApproval(this ICanAction approvalFlow, Employee employee, string comments)
        {
            return approvalFlow.RequestApproval(employee.Username, comments);
        }

        public static ICanAction Invalidate(this ICanAction approvalFlow, Employee employee, string comments = null)
        {
            return approvalFlow.Invalidate(employee.Username, comments);
        }

        public static ICanAction SetApp(this ICanAction approvalFlow, string app)
        {
            return approvalFlow.SetMetadata("app", app);
        }

        public static ICanAction LoadNotification(this ICanAction approvalFlow, Employee approver, IEnumerable<Employee> employeesCarbonCopy = null)
        {
            if (employeesCarbonCopy == null)
                return approvalFlow.LoadNotification(approver.Username);
            else
                return approvalFlow.LoadNotification(approver.Username, employeesCarbonCopy.Select(e => e.Username).ToArray());
        }

        public static void FireNotifications(this ICanAction approvalFlow)
        {
            var notifications = approvalFlow.GetPAFNotifications() ?? new List<PAFNotification>();
            var empProvider = EmployeeProvider.GetEmployeeProvider();
            var portalPath = ConfigurationManager.AppSettings["PortalPath"];
            approvalFlow.SetEntityMetaData();
            var path = approvalFlow.GetMetadata("path");
            var application = approvalFlow.GetMetadata("app");
            string longDescription = approvalFlow.GetMetadata("longDescription");
            string shortDescription = approvalFlow.GetMetadata("shortDescription");
            string objID = approvalFlow.GetMetadata("id");

            List<EmailMessage> mailList = new List<EmailMessage>();
            List<string> CCs = new List<string>();

            if (notifications != null)
            {
                if (notifications.Any())
                {
                    foreach (var item in notifications)
                    {
                        var sender = item.From == null ? null : empProvider.GetUserData(item.From);
                        var recipient = empProvider.GetUserData(item.To);

                        if (sender != null)
                        {
                            CCs.Add(sender.Mail);
                        }
                        if (item.UsersToCC != null)
                        {
                            CCs.AddRange(item.UsersToCC.Select(e => empProvider.GetUserData(e).Mail).Distinct().ToList());
                        }

                        string msg;
                        string alertMessage = "";
                        string decisionDisplay = item.DecisionType.GetDisplay();
                        switch (item.DecisionType)
                        {
                            case DecisionType.AwaitingDecision:
                                msg = "Your Action is Required.";
                                if (sender != null)
                                {
                                    msg = msg + " Requested by " + sender.DisplayName + ".";
                                    alertMessage = ". Requested by " + sender.DisplayName + ".";
                                }
                                if (item.Comments != null)
                                    msg = msg + "<br/>" + longDescription + "<br/><br/><p>Comments : <br/>" + item.Comments + "</p>";

                                break;

                            case DecisionType.Approved:
                                msg = "Document Approved";
                                if (sender != null)
                                {
                                    msg = msg + " by " + sender.DisplayName;
                                    alertMessage = ", By " + sender.DisplayName + ".";
                                }
                                msg += ".";
                                if (item.Comments != null)
                                    msg = msg + "<br/>" + longDescription + "<br/><br/><p>Comments : <br/>" + item.Comments + "</p>";
                                break;

                            case DecisionType.Rejected:
                                msg = "Document Rejected";
                                if (sender != null)
                                {
                                    msg = msg + " by " + sender.DisplayName;
                                    alertMessage = ", By " + sender.DisplayName + ".";
                                }
                                msg += ".";
                                if (item.Comments != null)
                                    msg = msg + "<br/>" + longDescription + "<br/><br/><p>Comments : <br/>" + item.Comments + "</p>";
                                break;

                            case DecisionType.Invalidated:
                                msg = "Your request to review is invalidated.";
                                alertMessage = "";
                                if (item.Comments != null)
                                    msg = msg + "<br/>" + longDescription + "<br/><br/><p>Comments : <br/>" + item.Comments + "</p>";
                                break;

                            default:
                                msg = "";
                                break;
                        }

                        var mail = new EmailMessage()
                        {
                            Sender = "placeholder@nsc.om",
                            RecipientName = recipient.DisplayName,
                            Recipient = recipient.Mail,
                            CCRecipients = CCs.ToArray(),
                            Subject = $"[{application}] {shortDescription}, Approval Notification: {decisionDisplay}",
                            Body = msg + "<br/><a href=\"" + portalPath + path + "/Index/" + objID + "\"/>Open Document</a><br/><br/>"
                        };

                        mailList.Add(mail);

                        if (sender != null)
                        {
                            alertMessage = shortDescription + " Approval Notification: " + decisionDisplay + alertMessage;

                            var alert = RegisterAlert.ForApp(application)
                                 .WithCategory("Approval")
                                 .ByUser(sender.Username)
                                 .ToTargetEmployees(new string[] { recipient.Username });

                            if (item.DecisionType == DecisionType.AwaitingDecision)
                                alert = alert.IsMajor();
                            else
                                alert = alert.IsInformation();

                            alert.WithMessage(alertMessage)
                                .WithURL(portalPath + path + "/Index/" + objID)
                                .PushAlert();
                        }
                    }

                    using (var email = new EmailHandler(mailList))
                        email.Send();

                    approvalFlow.ClearNotifications();
                }
            }
        }

        public static void SendReminders(this ICanAction approvalFlow)
        {
            var application = approvalFlow.GetMetadata("app");
            var empProvider = EmployeeProvider.GetEmployeeProvider();
            var portalPath = ConfigurationManager.AppSettings["PortalPath"];

            var obj = (IApprovalFlow<ITransition>)approvalFlow;
            var path = approvalFlow.GetMetadata("path");
            string longDescription = approvalFlow.GetMetadata("longDescription");
            string shortDescription = approvalFlow.GetMetadata("shortDescription");
            string objID = approvalFlow.GetMetadata("id");

            var transitionsAwaitingDecision = obj.Transitions.Where(t => t.ApproverDecision == DecisionType.AwaitingDecision && t.RequestedDate < DateTime.Now.AddDays(-2)).OrderByDescending(t => t.Order).ToList();

            foreach (var transition in transitionsAwaitingDecision)
            {
                Employee u1 = empProvider.GetUserData(transition.ApproverID);
                Employee u2 = empProvider.GetUserData(transition.RequesterID);

                string msg = "Your Action is Required. Requested by " + u2.DisplayName + ".";
                if (transition.RequesterComments != null)
                    msg = msg + "<br/><br/><p>Comments : <br/>" + transition.RequesterComments + "</p>";

                var mail = new EmailMessage()
                {
                    Sender = "placeholder@nsc.om",
                    RecipientName = u1.DisplayName,
                    Recipient = u1.Mail,
                    CCRecipients = new string[] { u2.Mail },
                    Subject = "(" + application + ") " + shortDescription + ", Reminder Notification: " + transition.ApproverDecision.GetDisplay(),
                    Body = msg + "<br/><a href=\"" + portalPath + path + "/Index/" + obj.GetID() + "\"/>Open Document</a><br/><br/>"
                };

                if (!(transition.ApproverID == "52980409" && path == "/Procurement/Requisitions"))
                {
                    using (var email = new EmailHandler(mail))
                        email.Send();

                    RegisterAlert.ForApp(application)
                        .WithCategory("Approval Reminder")
                        .ByUser(transition.RequesterID)
                        .ToTargetEmployees(new string[] { transition.ApproverID })
                        .IsMajor()
                        .WithMessage(shortDescription + ", Approval Notification: " + transition.ApproverDecision.GetDisplay() + ". Requested by " + u2.DisplayName + ".")
                        .WithURL(portalPath + path + "/Index/" + objID)
                        .PushAlert();
                }
            }
        }

        public static bool IsApproved(this IEnumerable<ITransition> transitions, Employee[] employees)
        {
            var selectedT = transitions.Where(t => employees.Contains(t.Approver));
            if (!selectedT.Any())
                return false;

            return selectedT.Where(t => t.ApproverDecision != DecisionType.Invalidated).All(t => t.ApproverDecision == DecisionType.Approved);
        }

        public static bool IsReapprovalNeeded(this IEnumerable<ITransition> transitions, Employee user)
        {
            var currentT = getCurrentUserTransition(transitions, user).OrderByDescending(o => o.Order).FirstOrDefault();

            if (currentT == null || currentT.ApproverDecision == DecisionType.Approved)
                return false;

            var order = currentT.Order + 1;

            var tr = transitions.Where(t => t.Order == order).FirstOrDefault();

            return tr != null ? (tr.ApproverDecision == DecisionType.Rejected) : false;
        }

        private static IEnumerable<ITransition> getCurrentUserTransition(IEnumerable<ITransition> transitions, Employee user)
        {
            return transitions?.Where(t => t.ApproverID == user.Username) ?? new List<ITransition>();
        }

        //public static ICanAction ReConsider(this ICanAction approvalFlow, string Comments = null)
        //{
        //    var _transition = _approvalFlow.Transitions.Where(t => t.AuthorizedSignatoryDecision == DecisionType.Rejected).OrderByDescending(t => t.Order).FirstOrDefault();

        //    Employee u1 = _transition.AuthorizedSignatory;
        //    Employee u2 = _transition.RequestedBy;
        //    string path = "";//getDocumentPath();
        //    approvalFlow.Transitions.Where(t => t.AuthorizedSignatoryID == _user).FirstOrDefault().AcknowledgementDate = DateTime.Now;

        //    var mail = new EmailMessage()
        //    {
        //        Sender = _application.SenderEmail,

        //        RecipientName = u1.DisplayName,
        //        Recipient = u1.Mail,
        //        CCRecipients = new string[] { u2.Mail },

        //        Subject = "(" + _application.ApplicationDisplayName + ") " + _approvalFlow.GetShortDescription() + ", Re-Approval Notification",

        //        Body = "Please reapprove the document<br/><a href=\"" + portalPath + path + "/Index/" + _approvalFlow.GetID() + "\"/>Open Document</a><br/><br/>"
        //    };

        //    using (var email = new EmailHandler(mail))
        //        email.Send();

        //    RegisterAlert.ForApp(_application.ApplicationDisplayName)
        //        .WithCategory("ReApproval Reminder")
        //        .ByUser(_transition.RequestedByID)
        //        .ToTargetEmployees(new string[] { _transition.AuthorizedSignatoryID })
        //        .IsMajor()
        //        .WithMessage(_approvalFlow.GetShortDescription() + ", Re-Approval Notification requested by " + u2.Mail)
        //        .WithURL(portalPath + path + "/Index/" + _approvalFlow.GetID())
        //        .PushAlert();

        //    return this;
        //}
    }
}