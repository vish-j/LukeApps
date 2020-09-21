using System.Web.Optimization;

namespace LukePurchaseSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/validation.extend.js",
                        "~/Scripts/labelRequired.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/ESignature").Include(
                      "~/Scripts/SignaturePad/signature_pad.umd.js",
                      "~/Scripts/SignaturePad/app.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/ESignature").Include(
                      "~/Content/SignaturePad/signature-pad.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/animation").Include(
          "~/Content/animate.css"
          ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-notify").Include(
    "~/Scripts/bootstrap-notify/bootstrap-notify.js",
    "~/Scripts/bootstrap-notify/notify-defaults.js"
    ));
            bundles.Add(new ScriptBundle("~/bundles/auditTrail").Include(
               "~/Scripts/Helpers/auditTrail.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/collectionItems").Include(
                "~/Scripts/Helpers/collectionItems.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/lumpsum").Include(
                "~/Scripts/Helpers/lumpsum.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/Scripts/moment.js",
                "~/Scripts/momentConfig/config.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/pdfmake").Include(
                "~/Scripts/pdfmake/pdfmake.js",
                "~/Scripts/pdfmake/vfs_fonts.js"
                ));

            bundles.Add(new Bundle("~/bundles/datatables")
                .Include(
                    "~/Scripts/datatables/DataTables/jquery.dataTables-{version}.js",
                    "~/Scripts/datatables/DataTables/dataTables.bootstrap-{version}.js",
                    "~/Scripts/datatables/JSZip/jszip-{version}.js",
                    "~/Scripts/datatables/Buttons/dataTables.buttons-{version}.js",
                    "~/Scripts/datatables/Buttons/buttons.bootstrap-{version}.js",
                    "~/Scripts/datatables/Buttons/buttons.colVis-{version}.js",
                    "~/Scripts/datatables/Buttons/buttons.html5-modded-{version}.js",
                    "~/Scripts/datatables/Buttons/buttons.print-{version}.js",
                    "~/Scripts/datatables/RowGroup/dataTables.rowGroup-{version}.js",
                    "~/Scripts/datatables/RowGroup/rowGroup.bootstrap-{version}.js",
                    "~/Scripts/datatables/Scroller/dataTables.scroller-{version}.js",
                    "~/Scripts/datatables/Scroller/scroller.bootstrap-{version}.js",
                    "~/Scripts/datatables/Select/dataTables.select-{version}.js",
                    "~/Scripts/datatables/Select/select.bootstrap-{version}.js",
                    "~/Scripts/moment.js",
                    "~/Scripts/momentConfig/config.js",
                    "~/Scripts/jquery-markjs/jquery.mark.js",
                    "~/Scripts/jquery-markjs/datatables.mark.js",
                    "~/Scripts/jquery-markjs/markjs-initialize.js"
            ));

            bundles.Add(new StyleBundle("~/Content/dt-basic")
                .Include(
                    "~/Content/datatables/DataTables/dataTables.bootstrap-{version}.css",
                    "~/Content/datatables/Buttons/buttons.bootstrap-{version}.css",
                    "~/Content/datatables/RowGroup/rowGroup.bootstrap-{version}.css",
                    "~/Content/datatables/Scroller/scroller.bootstrap-{version}.css",
                    "~/Content/datatables/Select/select.bootstrap-{version}.css",
                    "~/Content/jquery-markjs/datatables.mark.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/indexHelper").Include(
                            "~/Scripts/Helpers/index.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/dt-button").Include(
                "~/Scripts/datatablesExtensions/buttons.initialize.js"
                ));

            bundles.Add(new StyleBundle("~/Content/bdp").Include(
                "~/Content/bootstrap-dp/bootstrap-datepicker3.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-dp").Include(
                "~/Scripts/bootstrap-dp/bootstrap-datepicker.js",
                "~/Scripts/bootstrap-dp/dpDefaults.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-dropdown").Include(
                "~/Scripts/bootstrap-chosen/chosen.jquery.js"
                ));

            bundles.Add(new StyleBundle("~/Content/bootstrap-dropdown").Include(
                "~/Content/bootstrap-chosen/bootstrap-chosen.css",
                "~/Content/chosen-sprite.png",
                "~/Content/chosen-sprite@2x.png"
                ));

            bundles.Add(new ScriptBundle("~/bundles/alertHandling").Include(
                "~/Scripts/alertHandling/alertHandling.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/File-Upload").Include(
                "~/Scripts/jQuery-File-Upload/jquery.ui.widget.js",
                "~/Scripts/jQuery-File-Upload/jquery.fileupload.js",
                "~/Scripts/jQuery-File-Upload/jquery.iframe-transport.js",
                "~/Scripts/FileUploader/fileUploadControl.js"
                ));

            bundles.Add(new StyleBundle("~/Content/File-Upload").Include(
                "~/Content/FileUploader/fileUploadControl.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/purchaseOrderPDF").Include(
                "~/Scripts/pdfDefinitions/purchaseOrder.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/expenseClaimPDF").Include(
                "~/Scripts/pdfDefinitions/expenseClaim.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bidSummary").Include(
                "~/Scripts/pdfDefinitions/bidSummary.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/timeline").Include(
                "~/Scripts/timeline/timeline.js"
                ));

            bundles.Add(new StyleBundle("~/Content/timeline").Include(
                "~/Content/timelineCss/timeline.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/pdfmake").Include(
                "~/Scripts/pdfmake/pdfmake.js",
                "~/Scripts/pdfmake/vfs_fonts.js"
                ));
        }
    }
}