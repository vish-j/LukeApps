using HtmlHelpers.BeginCollectionItem;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.TrackingExtended;
using PhilApprovalFlow;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace LukePurchaseSystem.Controllers
{
    public class ExtendedController : Controller
    {
        protected void BuildViewBag(string ErrorMessage = null, long? id = null)
        {
            if (id != null)
                ViewBag.ModalID = id;
            if (ErrorMessage != null)
                ViewBag.ErrorMessage = ErrorMessage;
        }

        protected ActionResult AddNewChild<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, TValue>> selector, TValue value) where TEntity : class, IEntity
        {
            if (selector.Body is MemberExpression memberSelectorExpression)
            {
                PropertyInfo property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(entity, value, null);
                }
            }

            return AddNewChild(entity);
        }

        protected ActionResult AddNewChild<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, TValue>> selector, TValue value, string containerPrefix) where TEntity : class, IEntity
        {
            ViewData["ContainerPrefix"] = containerPrefix;
            return AddNewChild(entity, selector, value);
        }

        protected ActionResult AddNewChild<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return PartialView(
                viewName: entity.GetPartialViewName(),
                model: entity
            );
        }

        protected static void ProcessApprovals(IPAFMatrix entity, string username, ICanAction approval, string comments = null)
        {
            if (username == entity.OriginatorID)
            {
                approval.RequestApproval(entity.OriginatorID, "Originator");
                if (entity.ReviewerID != null)
                {
                    approval.RequestApproval(entity.ReviewerID, "Reviewer").LoadNotification(username, new string[] { entity.OriginatorID });
                }
                else
                {
                    approval.RequestApproval(entity.ApproverID, "Approver").LoadNotification(username, new string[] { entity.OriginatorID });
                }
            }
            else if (username == entity.ReviewerID)
            {
                approval.RequestApproval(entity.ApproverID, "Approver").LoadNotification(username, new string[] { entity.OriginatorID });
            }
            else
            {
                approval.RequestApproval(entity.OriginatorID, "Originator");
            }

            approval.Approve(comments)
                .LoadNotification(username, new string[] { entity.OriginatorID });


        }
    }
}