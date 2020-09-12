using LukeApps.ApprovalProcess;
using LukeApps.ApprovalProcess.ViewModels;
using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.Common.Helpers;
using LukeApps.EmployeeData;
using LukeApps.GeneralPurchase.Classes;
using LukeApps.GeneralPurchase.DAL;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.GeneralPurchase.Models;
using LukeApps.GeneralPurchase.ViewModel;
using LukeApps.GenericRepository;
using PhilApprovalFlow;
using PhilApprovalFlow.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LukePurchaseSystem.Controllers
{
    public class EnquiriesController : ExtendedController
    {
        private GenericRepository<PurchaseEntities, Enquiry> repo;
        private string validationErrormessage;

        public EnquiriesController()
        {
            repo = new GenericRepository<PurchaseEntities, Enquiry>(System.Web.HttpContext.Current.User.Identity.Name);
        }

        public EnquiriesController(string Username)
        {
            repo = new GenericRepository<PurchaseEntities, Enquiry>(Username);
        }

        // GET: Enquiries
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult Index(string ErrorMessage = null, long? id = null)
        {
            BuildViewBag(ErrorMessage, id);
            return View();
        }

        //fast json export to client
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<JsonResult> GetJSON()
        {
            var enquiries = await repo.GetAll().Include(e => e.Budget).ToListAsync();

            var detailCollection = enquiries.Select(e => new
            {
                EnquiryID = e.EnquiryID,
                EnquiryDate = e.EnquiryDate.ToShortDateISO(),
                SubjectOfEnquiry = e.SubjectOfEnquiry,
                BudgetID = e.BudgetID,
                OriginatorID = e.OriginatorID,
                TechnicalEvaluatorID = e.TechnicalEvaluatorID,
                ReviewerID = e.ReviewerID,
                ApproverID = e.ApproverID,
                RecommendedOfferID = e.RecommendedOfferID,
                RecommendationReason = e.RecommendationReason,
                SupportingDocuments = e.SupportingDocuments,
                AuditDetail_CreatedDate = e.AuditDetail.CreatedDate.ToShortDateISO(),
                AuditDetail_CreatedEntryUser = e.AuditDetail.CreatedEntryUserDisplayName,
                AuditDetail_LastModifiedDate = e.AuditDetail.LastModifiedDate?.ToShortDateISO(),
                AuditDetail_LastModifiedEntryUser = e.AuditDetail.LastModifiedEntryUserDisplayName
            });

            var TotalRecords = detailCollection.Count();
            return Json(new
            {
                iTotalRecords = TotalRecords,
                iTotalDisplayRecords = TotalRecords,
                aaData = detailCollection
            },
            JsonRequestBehavior.AllowGet);
        }

        // GET: Enquiries/Details/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == id);

            if (enquiry == null)
            {
                return HttpNotFound();
            }
            var currentUser = User.GetUserData();

            ViewBag.IsApprovedEnabled = enquiry.Transitions.IsApprovedEnabled(currentUser.Username);

            ViewBag.IsRejectEnabled = enquiry.Transitions.IsRejectEnabled(currentUser.Username);

            ViewBag.CanInvalidate = User.IsInRole(Role.Dev) || User.IsInRole(Role.SOFocalPoint);

            ViewBag.IsInTransitions = enquiry.Transitions.Any(t => t.ApproverID == currentUser.Username);

            return PartialView(enquiry);
        }

        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult AddNewScopeItem(long id)
        {
            return AddNewChild(new ScopeItem(), r => r.ScopeItemID, id);
        }

        private void viewBagBudgetID() => ViewBag.BudgetID = new SelectList(repo.Context.Budgets, "BudgetID", "BudgetName");

        // GET: Enquiries/Create
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult Create()
        {
            viewBagBudgetID();
            return View(new Enquiry());
        }

        // POST: Enquiries/Create

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Enquiry eq)
        {
            if (ModelState.IsValid)
            {
                if (eq.SupportingDocuments.IsAnyFilePresent)
                {
                    var totalContent = eq.SupportingDocuments.GetFilesInformation().Sum(e => e.Size);
                    if (totalContent > 14680064)
                    {
                        ModelState.AddModelError("Attachements", "Attached files are greater that 14MB.");
                        viewBagBudgetID();
                        return View(eq);
                    }
                }

                foreach (var companyID in eq.VendorEnquiriesList)
                {
                    eq.Offers.Add(new Offer()
                    {
                        IsNew = true,
                        CompanyID = long.Parse(companyID),
                        VendorResponse = VendorResponse.NoResponse,
                        AgreedPaymentTerms = new PaymentTerms
                        {
                            Term = 90,
                            Entitlement = ""
                        }
                    });
                }

                repo.Add(eq);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = eq.EnquiryID });
            }

            viewBagBudgetID();

            return View(eq);
        }

        // GET: Enquiries/Edit/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Edit(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == id);
            if (enquiry == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            viewBagBudgetID();
            return View(enquiry);
        }

        // POST: Enquiries/Edit/5
        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Enquiry eq)
        {
            if (ModelState.IsValid)
            {
                var preEnquiry = repo.FindBy(e => e.EnquiryID == eq.EnquiryID);

                preEnquiry.EnquiryDate = eq.EnquiryDate;
                preEnquiry.SubjectOfEnquiry = eq.SubjectOfEnquiry;
                preEnquiry.OriginatorID = eq.OriginatorID;
                preEnquiry.TechnicalEvaluatorID = eq.TechnicalEvaluatorID;
                preEnquiry.ReviewerID = eq.ReviewerID;
                preEnquiry.ApproverID = eq.ApproverID;
                preEnquiry.RecommendedOfferID = eq.RecommendedOfferID;
                preEnquiry.RecommendationReason = eq.RecommendationReason;
                preEnquiry.SupportingDocuments = eq.SupportingDocuments;

                repo.Edit(preEnquiry);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = preEnquiry.EnquiryID });
            }
            viewBagBudgetID();

            return View(eq);
        }

        // GET: Enquiries/Delete/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Delete(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == id);
            if (enquiry == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(enquiry);
        }

        // POST: Enquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == id);

            repo.Delete(enquiry);
            await repo.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetBidSummaryJSON(long? id)
        {
            if (modelID.check(id))
            {
                return Json("Invalid ID", JsonRequestBehavior.AllowGet);
            }

            Enquiry enquiry = await repo.Context.Enquiries.Include(e => e.Offers).Include(e => e.Transitions).FirstOrDefaultAsync(e => e.EnquiryID == id);

            if (enquiry == null)
            {
                return Json("Bad ID", JsonRequestBehavior.AllowGet);
            }

            if (enquiry.StatusBidSummary != StatusBidSummary.InProcess)
            {
                enquiry.StatusBidSummary = StatusBidSummary.InProcess;
                repo.Context.Entry(enquiry).State = EntityState.Modified;
                await repo.Context.SaveChangesAsync();
            }

            var bs = new BidSummary(enquiry);

            return Json(new
            {
                Table = bs.Table,
                CompanyCount = enquiry.Offers.Where(o => o.IsNew).Count(),
                ColumnWidths = bs.ColumnWidths,
                RequestedBy = enquiry.Originator.DisplayName,
                CheckedBy = enquiry.Reviewer.DisplayName,
                ApprovedBy = enquiry.Approver.DisplayName,
                Reviewers = enquiry.Transitions.Where(p => p.ApproverDecision != DecisionType.Invalidated).Select(t => new ReviewerVM(t)),
            },
            JsonRequestBehavior.AllowGet);
        }

        #region offer

        public async Task<ActionResult> OfferDetails(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = await repo.Context.Offers.Include(o => o.Enquiry).Include("Enquiry.Transitions").FirstOrDefaultAsync(o => o.OfferID == id);

            if (offer == null)
            {
                return HttpNotFound();
            }
            return PartialView(new OfferDisplayVM(offer));
        }

        // GET: SmallOrders/Offers/EnquireNewVendor/5

        public ActionResult EnquireNewVendor(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "Invalid ID" });
            }
            Enquiry enquiry = repo.Context.Enquiries.Find(id);
            if (enquiry == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            var e = new EnquireNewVendorVM()
            {
                EnquiryID = enquiry.EnquiryID,
                Companies = new SelectList(repo.Context.Companies.Where(c => !c.Offers.Any(o => o.EnquiryID == enquiry.EnquiryID)).ToList().Where(c => (c.CompanyStatus == CompanyStatus.Registered)).ToList(), "CompanyID", "CompanyName", "SelectListItemDisplay", null, null)
            };
            return View(e);
        }

        // POST: SmallOrders/Offers/EnquireNewVendor/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnquireNewVendor(EnquireNewVendorVM erequest)
        {
            if (ModelState.IsValid)
            {
                Enquiry enquiry = repo.Context.Enquiries.Find(erequest.EnquiryID);
                if (enquiry == null)
                {
                    return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
                }
                else
                {
                    var offer = new Offer
                    {
                        EnquiryID = enquiry.EnquiryID,
                        CompanyID = erequest.CompanyID,
                        VendorResponse = erequest.VendorResponse,
                        AgreedPaymentTerms = new PaymentTerms { Term = 90, Entitlement = "" },
                        IsNew = true,
                        Enquiry = enquiry
                    };

                    repo.Context.Offers.Add(offer);
                    await repo.Context.SaveChangesAsync();

                    return RedirectToAction("Index", new { id = erequest.EnquiryID });
                }
            }

            ViewBag.Companies = new SelectList(repo.Context.Companies.Where(c => !c.Offers.Any(o => o.EnquiryID == erequest.EnquiryID)).ToList().Where(c => (c.CompanyStatus == CompanyStatus.Registered)).ToList(), "CompanyID", "CompanyName", "SelectListItemDisplay", erequest.CompanyID, null);

            return View(erequest);
        }

        public async Task<ActionResult> EditOffer(long? id)
        {
            if (modelID.check(id))
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });

            Offer offer = await repo.Context.Offers.Include(o => o.Enquiry).Include(o => o.ScopeItems).FirstOrDefaultAsync(o => o.OfferID == id);

            if (offer == null)
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });

            if (offer.Enquiry.Transitions.IsAnyApproved())
            {
                return RedirectToAction("Index", new { ErrorMessage = "Workflow has Started" });
            }

            if (offer.VendorResponse == VendorResponse.NoResponse)
                offer.VendorResponse = VendorResponse.Responded;

            if (!offer.IsNew)
                viewBagLatestid(offer);

            return View(offer);
        }

        private long getLatestid(Offer offer) =>
            repo.Context.Offers.FirstOrDefault(e => e.EnquiryID == offer.EnquiryID && e.CompanyID == offer.CompanyID && e.IsNew).OfferID;

        private void viewBagLatestid(Offer offer) =>
            ViewBag.latestid = getLatestid(offer);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOffer(Offer ofr)
        {
            if (ModelState.IsValid)
            {
                var preOffer = repo.Context.Offers.Include(o => o.ScopeItems).Where(o => o.OfferID == ofr.OfferID).FirstOrDefault();

                preOffer.EnquiryID = ofr.EnquiryID;
                preOffer.CompanyID = ofr.CompanyID;
                preOffer.VendorResponse = ofr.VendorResponse;
                preOffer.BidReceivedDate = ofr.BidReceivedDate;
                preOffer.ExpiryDate = ofr.ExpiryDate;
                preOffer.DeliveryTerms = ofr.DeliveryTerms;
                preOffer.DeliveryDate = ofr.DeliveryDate;

                preOffer.Quotation = ofr.Quotation;

                preOffer.AgreedPaymentTerms = ofr.AgreedPaymentTerms;

                preOffer.ReferenceNumber = ofr.ReferenceNumber;

                repo.Context.Entry(preOffer).State = EntityState.Modified;

                repo.UpdateChildCollection(p => p.ScopeItems, preOffer, ofr);

                await repo.SaveChangesAsync();

                return preOffer.IsNew
                    ? RedirectToAction("Index", new { id = preOffer.EnquiryID })
                    : RedirectToAction("OfferHistory", new { id = preOffer.OfferID, latestid = getLatestid(preOffer) });
            }

            return View(ofr);
        }

        // GET: SmallOrders/Enquiries/AwardOffer/5
        public async Task<ActionResult> AwardOffer(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }

            Enquiry enquiry = await repo.GetAll().Include(p => p.Transitions)
                .Include(p => p.Offers)
                .FirstOrDefaultAsync(f => f.EnquiryID == id);

            if (enquiry == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            if (enquiry.Offers.Where(o => o.IsNew).Count() == 1)
            {
                var onlyOffer = enquiry.Offers.First(o => o.IsNew);
                if (enquiry.RecommendedOfferID == null || enquiry.RecommendedOfferID != onlyOffer.OfferID)
                {
                    enquiry.RecommendedOfferID = onlyOffer.OfferID;
                    repo.Edit(enquiry);
                    repo.SaveChanges();
                }
            }

            if (checkValidity(enquiry, enquiry.RecommededOffer))
                return View(new DecisionVM()
                {
                    ID = enquiry.EnquiryID,
                    Display = enquiry.ScopeOfWork,
                    RequesterComments = enquiry.Transitions.Where(t => t.ApproverID == User.Identity.Name.Substring(6)).FirstOrDefault()?.ApproverComments
                });
            else
                return RedirectToAction("Index", new { ErrorMessage = validationErrormessage, id, IsOfferValid = false });
        }

        // POST: SmallOrders/Enquiries/AwardOffer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AwardOffer(DecisionVM decision)
        {
            Enquiry enquiry = await repo.Context.Enquiries.Include(p => p.Transitions).FirstOrDefaultAsync(f => f.EnquiryID == decision.ID);

            if (modelID.check(enquiry.RecommendedOfferID))
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            Offer offer = await repo.Context.Offers.Include(e => e.Enquiry).Include(e => e.Enquiry.Transitions).FirstOrDefaultAsync(e => e.OfferID == enquiry.RecommendedOfferID);

            if (offer == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }

            await award(decision, offer);

            return RedirectToAction("Index", new { id = decision.ID });
        }

        // GET: SmallOrders/Enquiries/Recommend/5
        public async Task<ActionResult> Recommend(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "Invalid ID" });
            }
            Offer offer = await repo.Context.Offers.Include(e => e.Enquiry)
                .Include("Enquiry.PurchaseRequisition")
                .Include("Enquiry.Transitions")
                .Include("Enquiry.PurchaseRequisition.Transitions")
                .FirstOrDefaultAsync(e => e.OfferID == id);
            if (offer == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return checkValidity(offer.Enquiry, offer)
                ? View(new RecommendOfferVM(offer, id))
                : (ActionResult)RedirectToAction("Index", new { ErrorMessage = validationErrormessage, offer.EnquiryID, IsOfferValid = false });
        }

        // POST: SmallOrders/Enquiries/Recommend/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Recommend([Bind(Include = "EnquiryID,RecommendedOfferID,RecommendationReason")] RecommendOfferVM offer)
        {
            if (ModelState.IsValid)
            {
                //Enquiry
                var PreEnquiry = repo.Context.Enquiries.Find(offer.EnquiryID);

                PreEnquiry.RecommendedOfferID = offer.RecommendedOfferID;
                PreEnquiry.RecommendationReason = offer.RecommendationReason;

                repo.Context.Entry(PreEnquiry).State = EntityState.Modified;

                await repo.Context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = PreEnquiry.EnquiryID });
            }

            return View(offer);
        }

        // GET: SmallOrders/Offers/ReviseOffer/5
        public async Task<ActionResult> ReviseOffer(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "Invalid ID" });
            }

            Offer offer = await repo.Context.Offers.FindAsync(id);

            if (offer == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            ViewBag.Company = offer.Company.CompanyName;
            return View(new ReviseOfferVM
            {
                ReferenceNumber = offer.ReferenceNumber,
                CompanyID = offer.CompanyID,
                CompanyName = offer.Company.CompanyName,
                VendorResponse = offer.VendorResponse,
                BidReceivedDate = offer.BidReceivedDate,
                ExpiryDate = offer.ExpiryDate,
                AgreedPaymentTerms = offer.AgreedPaymentTerms,
                DeliveryDate = offer.DeliveryDate,
                DeliveryTerms = offer.DeliveryTerms,
                PreviousOfferID = offer.OfferID,
                EnquiryID = offer.EnquiryID,
                ScopeItems = offer.ScopeItems.Select(a => { a.OfferID = 0; return a; }).ToList()
            });
        }

        // POST: SmallOrders/Offers/ReviseOffer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReviseOffer(ReviseOfferVM offer)
        {
            if (ModelState.IsValid)
            {
                Offer previousOffer = await repo.Context.Offers.Include(r => r.Enquiry).FirstOrDefaultAsync(r => r.OfferID == offer.PreviousOfferID);
                previousOffer.IsNew = false;
                repo.Context.Entry(previousOffer).State = EntityState.Modified;

                var o = new Offer
                {
                    PreviousOffer = previousOffer,
                    Revision = previousOffer.Revision + 1,
                    ReferenceNumber = offer.ReferenceNumber,
                    CompanyID = previousOffer.CompanyID,
                    EnquiryID = previousOffer.EnquiryID,
                    VendorResponse = offer.VendorResponse,
                    BidReceivedDate = offer.BidReceivedDate,
                    ExpiryDate = offer.ExpiryDate,
                    AgreedPaymentTerms = offer.AgreedPaymentTerms,
                    IsNew = true,
                    ScopeItems = offer.ScopeItems,
                    DeliveryDate = offer.DeliveryDate,
                    DeliveryTerms = offer.DeliveryTerms
                };

                repo.Context.Offers.Add(o);

                await repo.Context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = previousOffer.EnquiryID });
            }

            Offer oldoffer = await repo.Context.Offers.Include(o => o.Company).FirstOrDefaultAsync(o => o.OfferID == offer.PreviousOfferID);
            ViewBag.Company = oldoffer.Company.CompanyName;
            return View(offer);
        }

        // GET: SmallOrders/Offers/ReviseOffer/5
        public async Task<ActionResult> ReviseValidityOffer(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "Invalid ID" });
            }

            Offer offer = await repo.Context.Offers.FindAsync(id);

            if (offer == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            ViewBag.Company = offer.Company.CompanyName;
            return View(new ReviseOfferVM
            {
                ReferenceNumber = offer.ReferenceNumber,
                CompanyID = offer.CompanyID,
                CompanyName = offer.Company.CompanyName,
                VendorResponse = offer.VendorResponse,
                BidReceivedDate = offer.BidReceivedDate,
                ExpiryDate = offer.ExpiryDate,
                AgreedPaymentTerms = offer.AgreedPaymentTerms,
                DeliveryDate = offer.DeliveryDate,
                DeliveryTerms = offer.DeliveryTerms,
                PreviousOfferID = offer.OfferID,
                EnquiryID = offer.EnquiryID,
                ScopeItems = offer.ScopeItems.Select(a => { a.OfferID = 0; return a; }).ToList()
            });
        }

        // POST: SmallOrders/Offers/ReviseOffer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReviseValidityOffer(ReviseOfferVM offer)
        {
            if (ModelState.IsValid)
            {
                Offer previousOffer = await repo.Context.Offers.Include(o => o.Enquiry).Include("Enquiry.Transitions").FirstOrDefaultAsync(o => o.OfferID == offer.PreviousOfferID);
                var workflow = previousOffer.Enquiry.GetApprovalFlow();
                workflow.ResetTransitions("Expired Offer Revised");

                previousOffer.IsNew = false;
                repo.Context.Entry(previousOffer).State = EntityState.Modified;

                var newOffer = new Offer
                {
                    PreviousOffer = previousOffer,
                    Revision = previousOffer.Revision + 1,
                    ReferenceNumber = offer.ReferenceNumber,
                    CompanyID = previousOffer.CompanyID,
                    EnquiryID = previousOffer.EnquiryID,
                    VendorResponse = previousOffer.VendorResponse,
                    BidReceivedDate = offer.BidReceivedDate,
                    ExpiryDate = offer.ExpiryDate,
                    AgreedPaymentTerms = previousOffer.AgreedPaymentTerms,
                    IsNew = true,
                    ScopeItems = previousOffer.ScopeItems.Select(s => new ScopeItem
                    {
                        Description = s.Description,
                        IsLumpSum = s.IsLumpSum,
                        ScopeItemType = s.ScopeItemType,
                        Unit = s.Unit,
                        UnitPrice = s.UnitPrice,
                        Quantity = s.Quantity,
                        Order = s.Order
                    }).ToList(),
                    DeliveryDate = previousOffer.DeliveryDate,
                    DeliveryTerms = previousOffer.DeliveryTerms,
                    Quotation = previousOffer.Quotation,
                };

                repo.Context.Offers.Add(newOffer);

                await repo.Context.SaveChangesAsync();

                Enquiry enquiry = await repo.Context.Enquiries.FindAsync(previousOffer.EnquiryID);
                if (enquiry.RecommendedOfferID == previousOffer.OfferID)
                {
                    enquiry.RecommendedOfferID = newOffer.OfferID;
                    repo.Context.Entry(enquiry).State = EntityState.Modified;
                    await repo.Context.SaveChangesAsync();
                }

                return RedirectToAction("Index", new { id = previousOffer.EnquiryID });
            }

            Offer oldoffer = await repo.Context.Offers.Include(o => o.Company).FirstOrDefaultAsync(o => o.OfferID == offer.PreviousOfferID);
            ViewBag.Company = oldoffer.Company.CompanyName;
            return View(offer);
        }

        //fast json export to client
        public async Task<JsonResult> OfferHistorygetJSON(long? latestid)
        {
            if (modelID.check(latestid))
            {
                return Json(new
                {
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = new string[1]
                },
                JsonRequestBehavior.AllowGet);
            }
            Offer offer = await repo.Context.Offers.FindAsync(latestid);
            if (offer == null)
            {
                return Json(new
                {
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = new string[1]
                },
                JsonRequestBehavior.AllowGet);
            }
            var offers = repo.Context.Offers.Where(o => offer.OfferJourney.Contains(o.OfferID)).ToList();
            var detailCollection = offers.Select(o => new OfferDisplayVM(o)).Select(o => new
            {
                OfferNumber = o.OfferNumber,
                Company = o.CompanyName,
                Revision = o.Revision,
                VendorResponse = o.VendorResponse,
                BidReceivedDate = o.BidReceivedDate,
                TotalPriceQuoted = o.TotalPriceQuoted,
                ExpiryDate = o.ExpiryDate,
                InitialPaymentTerms = o.InitialPaymentTerms,
                IsTermsConditionsAgreed = o.IsTermsConditionsAgreed,
                DeviationsInTermsAndConditions = o.DeviationsInTermsAndConditions,
                IsPaymentTermsAgreed = o.IsPaymentTermsAgreed,
                AgreedPaymentTerms = o.AgreedPaymentTerms,
                Milestones = o.Milestones,
                ScheduleExecution = o.ScheduleExecution,
                IsTechnicallyAcceptable = o.IsTechnicallyAcceptable,
                IsCommerciallyAcceptable = o.IsCommerciallyAcceptable,
                CreatedDate = o.CreatedDate,
                CreatedEntryUser = o.CreatedEntryUser,
                LastModifiedDate = o.LastModifiedDate,
                LastModifiedEntryUser = o.LastModifiedEntryUser
            });
            var TotalRecords = detailCollection.Count();
            return Json(new
            {
                iTotalRecords = TotalRecords,
                iTotalDisplayRecords = TotalRecords,
                aaData = detailCollection
            },
            JsonRequestBehavior.AllowGet);
        }

        // GET: SmallOrders/Enquiries/OfferHistory
        public async Task<ActionResult> OfferHistory(long? id, long? latestid, string ErrorMessage = null)
        {
            if (modelID.check(latestid))
            {
                return RedirectToAction("Index", new { ErrorMessage = "Invalid ID" });
            }
            Offer offer = await repo.Context.Offers.FindAsync(latestid);

            if (offer == null || offer.IsNew == false)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            BuildViewBag(ErrorMessage, id);
            return View(new Offer { OfferID = offer.OfferID, EnquiryID = offer.EnquiryID });
        }

        // GET: SmallOrders/Enquiries/OfferDelete/5
        public async Task<ActionResult> OfferDelete(long? id)
        {
            if (modelID.check(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = await repo.Context.Offers.FindAsync(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: SmallOrders/Enquiries/OfferDelete/5
        [HttpPost, ActionName("OfferDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OfferDeleteConfirmed(long id)
        {
            Offer offer = await repo.Context.Offers.FindAsync(id);
            deleteOffer(offer);
            await repo.Context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = offer.EnquiryID });
        }

        private void deleteOffer(Offer offer)
        {
            repo.Context.Offers.Remove(offer);

            if (offer.PreviousOffer != null)
                deleteOffer(offer.PreviousOffer);
        }

        // GET: SmallOrders/Enquiries/OfferDelete/5
        public async Task<ActionResult> RevOfferDelete(long? id)
        {
            if (modelID.check(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = await repo.Context.Offers.FindAsync(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            viewBagLatestid(offer);
            return View(offer);
        }

        // POST: SmallOrders/Enquiries/OfferDelete/5
        [HttpPost, ActionName("RevOfferDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RevOfferDeleteConfirmed(long id)
        {
            Offer offer = await repo.Context.Offers.FindAsync(id);
            offer.PreviousOffer = null;

            int newRev = 0;
            var offers = repo.Context.Offers.Where(o => o.EnquiryID == offer.EnquiryID && o.CompanyID == offer.CompanyID && o.OfferID != offer.OfferID)
                .OrderBy(o => o.Revision)
                .AsEnumerable();
            var offersTotal = offers.Count();
            foreach (var item in offers)
            {
                item.Revision = newRev;
                newRev++;
                item.PreviousOffer = offers.Where(o => o.Revision == item.Revision - 1).FirstOrDefault();
                item.IsNew = item.Revision == (offersTotal - 1) ? true : false;
                repo.Context.Entry(item).State = EntityState.Modified;
            }

            repo.Context.Offers.Remove(offer);

            await repo.SaveChangesAsync();

            return RedirectToAction("OfferHistory", new { latestid = getLatestid(offer) });
        }

        private void addToValidationErrorMessage(string ErrorMessage) => validationErrormessage = validationErrormessage + "," + ErrorMessage;

        private bool checkValidity(Enquiry enquiry, Offer recommendedOffer)
        {
            validationErrormessage = "It is not possible to select this offer";
            bool IsValid = true;

            if ((recommendedOffer == null))
            {
                addToValidationErrorMessage("Recommended Offer does not exist");
                IsValid = false;
                return IsValid;
            }

            if (recommendedOffer.IsOfferExpired)
            {
                addToValidationErrorMessage("The Offer has expired");
                IsValid = false;
            }

            if (!recommendedOffer.Quotation.IsAnyFilePresent)
            {
                addToValidationErrorMessage("The Offer has no attached documents.");
                IsValid = false;
            }

            if (recommendedOffer.DeliveryDate == null || recommendedOffer.DeliveryTerms == null)
            {
                addToValidationErrorMessage("The Offer has missing delivery information");
                IsValid = false;
            }

            return IsValid;
        }

        private async Task award(DecisionVM decision, Offer offer)
        {
            var currentUser = User.GetUserData();

            PurchaseOrder agreement = new PurchaseOrder();

            agreement.PurchaseOrderDate = DateTime.Now;
            agreement.Revision = 0;
            agreement.SequenceNumber = (repo.Context.PurchaseOrders.AsEnumerable().Max(a => (int?)a.SequenceNumber) ?? 0) + 1;

            agreement.BudgetID = offer.Enquiry.BudgetID;
            agreement.CompanyID = offer.CompanyID;
            agreement.CompanySection = offer.Company.CompanyName + ",\n" + offer.Company.Address.ToString();
            agreement.CompanyContactName = offer.Company.DefaultFocalPoint.ContactName;
            agreement.OfferReference = offer.ReferenceNumber ?? "-";
            agreement.ShippingSection = "Tebodin & Partner LLC.,\n" + new Address
            {
                Line1 = "2nd Floor, Azaiba Plaza, Bldg. No.187",
                Line2 = "Way 61, Al-Ma'aridh St, Ghala",
                POBox = 716,
                PostalCode = 130,
                Street = "Al-Azaibah",
                City = "Muscat",
                Country = 0
            }.ToString();
            agreement.OriginatorID = offer.Enquiry.OriginatorID;
            agreement.OriginatorName = offer.Enquiry.Originator.DisplayName;
            agreement.OriginatorPhone = offer.Enquiry.Originator.TelephoneNumber ?? "-";
            agreement.TechnicalEvaluatorID = offer.Enquiry.TechnicalEvaluatorID;
            agreement.TechnicalEvaluatorName = offer.Enquiry.TechnicalEvaluator.DisplayName;
            agreement.CompanyID = offer.CompanyID;
            agreement.DeliveryDate = offer.DeliveryDate;
            agreement.DeliveryTerms = offer.DeliveryTerms;
            agreement.PaymentTerms = offer.AgreedPaymentTerms;
            agreement.ReviewerID = offer.Enquiry.Reviewer.Username;
            agreement.ReviewerName = offer.Enquiry.Reviewer.DisplayName;
            agreement.ReviewerPosition = $"{offer.Enquiry.Reviewer.JobTitle} - Tebodin Oman";
            agreement.ApproverID = offer.Enquiry.Approver.Username;
            agreement.ApproverName = offer.Enquiry.Approver.DisplayName;
            agreement.ApproverPosition = $"{offer.Enquiry.Approver.JobTitle} - Tebodin Oman";
            agreement.PurchaseOrderItems = offer.ScopeItems.Select(s => new PurchaseOrderItem
            {
                Description = s.Description,
                IsLumpSum = s.IsLumpSum,
                Order = s.Order,
                Quantity = s.Quantity,
                ScopeItemType = s.ScopeItemType,
                Unit = s.Unit ?? "-",
                UnitPrice = s.UnitPrice,
                AuditDetail = s.AuditDetail
            }).ToList();

            agreement.Offer = offer;

            var pr = EmployeeProvider.GetEmployeeProvider();
            var approval = agreement.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .RequestApproval(User.GetUserData())
                .Approve()
                .RequestApproval(agreement.ApproverID, decision.Comments)
                .LoadNotification(agreement.ApproverID, new string[] { agreement.OriginatorID });

            repo.Context.PurchaseOrders.Add(agreement);
            await repo.SaveChangesAsync();

            approval.FireNotifications();
        }

        #endregion offer

        #region #ApprovalProccess

        [HttpGet]
        public async Task<ActionResult> Approve(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == id, nameof(Enquiry.Transitions));
            if (enquiry == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(new DecisionVM()
            {
                ID = enquiry.EnquiryID,
                Display = enquiry.GetLongDescription(),
                RequesterComments = enquiry.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(DecisionVM decision)
        {
            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == decision.ID, nameof(Enquiry.Transitions));
            Employee currentemployee = User.GetUserData();

            var approval = enquiry.GetApprovalFlow()
                .SetUserName(currentemployee);

            ProcessApprovals(decision, enquiry, currentemployee.Username, approval);

            repo.Edit(enquiry);
            await repo.SaveChangesAsync();

            approval.FireNotifications();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Reject(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }

            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == id, nameof(Enquiry.Transitions));

            if (enquiry == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(new DecisionVM()
            {
                ID = enquiry.EnquiryID,
                Display = enquiry.GetLongDescription(),
                RequesterComments = enquiry.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reject(DecisionVM decision)
        {
            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == decision.ID, nameof(Enquiry.Transitions));

            var approval = enquiry.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .Reject(decision.Comments)
                .LoadNotification(User.GetUserData(), new List<Employee> { enquiry.Originator });

            repo.Edit(enquiry);
            await repo.SaveChangesAsync();

            approval.FireNotifications();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Invalidate(long? id, string username)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }

            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == id, nameof(Enquiry.Transitions));

            if (enquiry == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(new DecisionVM()
            {
                ID = enquiry.EnquiryID,
                Approver = username,
                Display = enquiry.GetLongDescription(),
                RequesterComments = enquiry.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invalidate(DecisionVM decision)
        {
            Enquiry enquiry = await repo.FindByAsync(e => e.EnquiryID == decision.ID, nameof(Enquiry.Transitions));

            var approval = enquiry.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .Invalidate(decision.ApproverEmployee, decision.Comments)
                .LoadNotification(decision.ApproverEmployee, new List<Employee> { enquiry.Originator });

            repo.Edit(enquiry);
            await repo.SaveChangesAsync();

            approval.FireNotifications();

            return RedirectToAction("Index", new { id = decision.ID });
        }

        #endregion #ApprovalProccess

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}