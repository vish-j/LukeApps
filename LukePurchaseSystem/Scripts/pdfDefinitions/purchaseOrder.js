function downloadOrder(id, rooturl) {
    var urlValue = rooturl + "GetPurchaseOrder?id=" + id;
    console.log(urlValue);
    var docDefinition =
    {
        footer: function (currentPage, pageCount) {
            return {
                columns: [
                    moment().format('MMMM Do YYYY, h:mm:ss a'),
                    {
                        alignment: 'right',
                        text: [
                            { text: currentPage.toString(), italics: true },
                            ' of ',
                            { text: pageCount.toString(), italics: true }
                        ]
                    }
                ],
                fontSize: 10,
                margin: [10, 20, 10, 10]
            };
        },
        header: function (currentPage, pageCount, pageSize) {
            return {
                columns: [
                    [
                        { text: 'NSC,', fontSize: 7, margin: [50, 20, 0, 0] },
                        { text: 'Al Hail,', fontSize: 7, margin: [50, 0] },
                        { text: 'Sultanate of Oman', fontSize: 7, margin: [50, 0] },
                    ]
                    ,
                    {
                        image: 'bilfinger',
                        width: 90,
                        alignment: 'right',
                        margin: [0, 20, 20, 0]
                    },
                ],
                fontSize: 10
            };
        },

        pageSize: 'A4',
        pageMargins: [40, 100, 40, 60],
        pageOrientation: 'potrait',
        watermark: { text: '   Cancelled   ', color: 'blue', opacity: 0.3, bold: true, italics: false },
        content: [
            {//0
                text: "Order Confirmation",
                "fontSize": 12,
                bold: true,
                margin: [10, 30, 0, 0],
            },
            {//1
                fontSize: 11,
                margin: [10, 10, 0, 0],
                table: {
                    // headers are automatically repeated if the table spans over multiple pages
                    // you can declare how many rows should be treated as headers
                    headerRows: 1,
                    dontBreakRows: true,
                    widths: [60, 60, 40],
                    body: [
                        [{ text: 'Order Number', "fontSize": 10, bold: true }, { text: 'PO Date', "fontSize": 10, bold: true }, { text: 'Rev ', "fontSize": 10, bold: true }],
                        [{ text: '-', "fontSize": 10, bold: true }, { text: '-', "fontSize": 10, bold: true }, { text: '- ', "fontSize": 10, bold: true }],
                    ]
                },
                layout: {
                    hLineWidth: function (i, node) {
                        return (i === 0 || i === 1 || i === node.table.body.length - 1 || i === node.table.body.length) ? 2 : 1;
                    },
                    vLineWidth: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 2 : 1;
                    },
                    hLineColor: function (i, node) {
                        return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                    },
                    vLineColor: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                    },
                    // paddingLeft: function(i, node) { return 4; },
                    // paddingRight: function(i, node) { return 4; },
                    paddingTop: function (i, node) { return 5; },
                    paddingBottom: function (i, node) { return 5; },
                    // fillColor: function (i, node) { return null; }
                }
            },
            {//2
                columns: [
                    { text: 'Vendor', "fontSize": 10, bold: true, width: 70 },
                    { text: '________', "fontSize": 10 },
                    { text: 'Ship To', "fontSize": 10, bold: true, width: 70 },
                    { text: '________', "fontSize": 10 },
                ],
                margin: [10, 20, 0, 0]
            },
            {//3
                columns: [
                    { text: 'Contact', "fontSize": 10, bold: true, width: 70 },
                    { text: '________', "fontSize": 10 },
                    { text: 'Contact', "fontSize": 10, bold: true, width: 70 },
                    { text: '________', "fontSize": 10 },
                ],
                margin: [10, 10, 0, 0]
            },
            {//4
                columns: [
                    { text: 'Reference', "fontSize": 10, bold: true, width: 70 },
                    { text: '________', "fontSize": 10 },
                    { text: 'Phone No.', "fontSize": 10, bold: true, width: 70 },
                    { text: '', "fontSize": 10 },
                ],
                margin: [10, 10, 0, 0]
            },
            {//5
                fontSize: 11,
                margin: [10, 20, 0, 0],
                table: {
                    // headers are automatically repeated if the table spans over multiple pages
                    // you can declare how many rows should be treated as headers
                    headerRows: 1,
                    dontBreakRows: true,
                    widths: ['*', 140, 160],
                    body: [
                        [{ text: 'Delivery Terms', "fontSize": 10, bold: true }, { text: 'Delivery Date', "fontSize": 10, bold: true }, { text: 'Inspected by ', "fontSize": 10, bold: true }],
                        [{ text: '-', "fontSize": 10, bold: true }, { text: '-', "fontSize": 10, bold: true }, { text: '- ', "fontSize": 10, bold: true }],
                    ]
                },
                layout: {
                    hLineWidth: function (i, node) {
                        return (i === 0 || i === 1 || i === node.table.body.length - 1 || i === node.table.body.length) ? 2 : 1;
                    },
                    vLineWidth: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 2 : 1;
                    },
                    hLineColor: function (i, node) {
                        return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                    },
                    vLineColor: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                    },
                    // paddingLeft: function(i, node) { return 4; },
                    // paddingRight: function(i, node) { return 4; },
                    paddingTop: function (i, node) { return 5; },
                    paddingBottom: function (i, node) { return 5; },
                    // fillColor: function (i, node) { return null; }
                }
            },
            {//6
                text: "The supplier is obliged to comply with the agreed Terms.",
                "fontSize": 10,
                margin: [10, 10, 0, 0],
            },
            {//7
                fontSize: 11,
                margin: [10, 20, 0, 0],
                table: {
                    // headers are automatically repeated if the table spans over multiple pages
                    // you can declare how many rows should be treated as headers
                    headerRows: 1,
                    dontBreakRows: true,
                    widths: [22, 40, '*', 60, 60],
                    body: [
                        [{ text: 'Item', "fontSize": 10, bold: true }, { text: 'Quantity', "fontSize": 10, bold: true }, { text: 'Description', "fontSize": 10, bold: true }, { text: 'Unit price', "fontSize": 10, bold: true }, { text: 'Total price', "fontSize": 10, bold: true }],
                    ]
                },
                layout: {
                    hLineWidth: function (i, node) {
                        return (i === 0 || i === 1 || i === node.table.body.length - 1 || i === node.table.body.length) ? 2 : 1;
                    },
                    vLineWidth: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 2 : 1;
                    },
                    hLineColor: function (i, node) {
                        return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                    },
                    vLineColor: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                    },
                    // paddingLeft: function(i, node) { return 4; },
                    // paddingRight: function(i, node) { return 4; },
                    paddingTop: function (i, node) { return 5; },
                    paddingBottom: function (i, node) { return 5; },
                    // fillColor: function (i, node) { return null; }
                }
            },
            {//8
                fontSize: 11,
                margin: [10, 0, 0, 0],
                table: {
                    // headers are automatically repeated if the table spans over multiple pages
                    // you can declare how many rows should be treated as headers
                    headerRows: 0,
                    dontBreakRows: true,
                    widths: [20, 40, '*', 60, 60],
                    body: [
                        [{ text: 'Total order value (excl. VAT)', "fontSize": 10, bold: true, colSpan: 4 }, {}, {}, {}, { text: '-', alignment: 'right', "fontSize": 10, bold: true }],
                        [{ text: 'Say : ', "fontSize": 8, colSpan: 2 }, {}, { text: '-', "fontSize": 10, alignment: 'center', colSpan: 3 }, {}, {}],
                        [{ text: 'Payment Term : ', "fontSize": 8, colSpan: 2 }, {}, { text: '-', "fontSize": 10, alignment: 'center', colSpan: 3 }, {}, {}],
                        //[{ text: 'Payment Method : ', "fontSize": 8, colSpan: 2 }, { }, { text: '-', "fontSize": 10, alignment: 'center', colSpan: 3 }, { }, { }],
                        [{ text: 'Milestone / Entitlement   : ', "fontSize": 8, colSpan: 2 }, {}, { text: '-', "fontSize": 10, alignment: 'center', colSpan: 3 }, {}, {}],
                    ]
                },
                layout: {
                    hLineWidth: function (i, node) {
                        return (i === 0 || i === 1 || i === node.table.body.length - 1 || i === node.table.body.length) ? 2 : 1;
                    },
                    vLineWidth: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 2 : 1;
                    },
                    hLineColor: function (i, node) {
                        return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                    },
                    vLineColor: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                    },
                    // paddingLeft: function(i, node) { return 4; },
                    // paddingRight: function(i, node) { return 4; },
                    paddingTop: function (i, node) { return 5; },
                    paddingBottom: function (i, node) { return 5; },
                    // fillColor: function (i, node) { return null; }
                }
            },
            {//9
                columns: [
                    { text: 'For Tebodin & Partner LLC', "fontSize": 10, bold: true },
                    {},
                    {},
                    {},
                    {},
                    {},
                    { text: 'For Supplier', "fontSize": 10, bold: true },
                    {},
                ],
                margin: [10, 20, 0, 0],
                pageBreak: 'before'
            },
            {//10
                columns: [
                    { text: 'Name', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                    { text: 'Name', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                    { text: '', "fontSize": 10, bold: true, width: 50 },
                    { text: '', "fontSize": 10 },
                    { text: 'Name', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                ],
                margin: [10, 20, 0, 0]
            },
            {//11
                columns: [
                    { text: 'Position', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                    { text: 'Position', "fontSize": 10, bold: true, width: 50 },
                    { text: 'Manager', "fontSize": 10 },
                    { text: '', "fontSize": 10, bold: true, width: 50 },
                    { text: '', "fontSize": 10 },
                    { text: 'Position', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },

                ],
                margin: [10, 10, 0, 0]
            },
            {//12
                columns: [
                    { text: '', "fontSize": 10, bold: true, width: 50 },
                    { image: 'AuthorizedPerson', fit: [80, 100], margin: [0, 0, 0, -100] },
                    { text: '', "fontSize": 10, width: 50 },
                    { image: 'ContentOwner', fit: [80, 100], margin: [0, 0, 0, -100] },
                    { text: '', "fontSize": 10, width: 50 },
                    { text: '', "fontSize": 10 },
                    { text: '', "fontSize": 10, width: 50 },
                    { text: '', "fontSize": 10 },
                ],
                margin: [10, 30, 0, 0]
            },
            {//13
                columns: [
                    { text: 'Signature', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                    { text: 'Signature', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                    { text: '', "fontSize": 10, bold: true, width: 50 },
                    { text: '', "fontSize": 10 },
                    { text: 'Signature', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                ],
                margin: [10, 30, 0, 0]
            },
            {//14
                columns: [
                    { text: 'Date', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                    { text: 'Date', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                    { text: '', "fontSize": 10, bold: true, width: 50 },
                    { text: '', "fontSize": 10 },
                    { text: 'Date', "fontSize": 10, bold: true, width: 50 },
                    { text: '____________', "fontSize": 10 },
                ],
                margin: [10, 10, 0, 0]
            },
            {//15
                text: "In consideration of the agreements herein contained, the Parties hereto agree as follows:",
                "fontSize": 10,
                margin: [10, 30, 0, 0],
            },
            {//16
                columns: [
                    { text: '1', "fontSize": 12, bold: true, width: 50 },
                    { text: 'Purchase Order Documents', bold: true, "fontSize": 12 },
                ],
                margin: [10, 10, 0, 0]
            }            
        ],
        styles: {
            header: {
                fontSize: 10,
                bold: true,
                margin: [10, 5, 10, 0]
            },
            bigger: {
                fontSize: 15,
                italics: true
            },
            subheader: {
                fontSize: 10,
                bold: true
            },
            quote: {
                italics: true
            },
            small: {
                fontSize: 8
            },
            normal: {
                fontSize: 9,
                margin: [0, 10, 10, 5]
            },
            columnNormal: {
                columnGap: 20
            },
        },
        images: {
            bilfinger: 'data:image/png;base64,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',
            ContentOwner: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8Xw8AAoMBgDTD2qgAAAAASUVORK5CYII=',
            AuthorizedPerson: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8Xw8AAoMBgDTD2qgAAAAASUVORK5CYII=',
        }
    };

    $.ajax({
        async: true,
        type: "GET",
        cache: false,
        url: urlValue
    }).done(function (data, textStatus, jqXHR) {
        docDefinition.content[1].table.body[1][0].text = data.OrderNumber;
        docDefinition.content[1].table.body[1][1].text = data.PODate;
        docDefinition.content[1].table.body[1][2].text = data.Revision;

        docDefinition.content[2].columns[1].text = data.VendorSection;
        docDefinition.content[2].columns[3].text = data.ShippingSection;

        docDefinition.content[3].columns[1].text = data.VendorContactName;
        docDefinition.content[3].columns[3].text = data.ProcurementContactName;

        docDefinition.content[4].columns[1].text = data.VendorReference;
        docDefinition.content[4].columns[3].text = data.ProcurementContactNumber;

        docDefinition.content[5].table.body[1][0].text = data.DeliveryTerms;
        docDefinition.content[5].table.body[1][1].text = data.DeliveryDate;
        docDefinition.content[5].table.body[1][2].text = data.InspectedBy;

        docDefinition.content[7].table.body[0][3].text = docDefinition.content[7].table.body[0][3].text + " (" + data.Currency + ")";
        docDefinition.content[7].table.body[0][4].text = docDefinition.content[7].table.body[0][4].text + " (" + data.Currency + ")";

        docDefinition.content[8].table.body[0][4].text = data.OrderTotal;
        docDefinition.content[8].table.body[1][2].text = data.OrderTotalWords;
        docDefinition.content[8].table.body[2][2].text = data.PaymentTerm === null ? '90' : data.PaymentTerm;

        docDefinition.content[8].table.body[3][2].text = data.Entitlement === null ? '-' : data.Entitlement;

        if (data.ApprovedBy !== null)
            docDefinition.content[10].columns[1].text = data.ApprovedBy;

        docDefinition.content[10].columns[3].text = data.ContentOwner;

        if (data.ApprovedPosition !== null)
            docDefinition.content[11].columns[1].text = data.ApprovedPosition;
        docDefinition.content[11].columns[3].text = data.ContentOwnerPosition;

        docDefinition.content[14].columns[1].text = data.AuthorisedPersonApprovedOn;
        docDefinition.content[14].columns[3].text = data.ContentOwnerApprovedOn;

        docDefinition.content[26].columns[1].text = 'This Agreement Number ' + data.OrderNumber;

        for (var o in data.OrderItems) {
            var dataArray = new Array;

            var value = data.OrderItems[o];
            dataArray.push({ text: (parseInt(o) + 1).toString(), "fontSize": 10 });
            dataArray.push({ text: value['Quantity'].toString() + ' ' + value['Unit'].toString(), "fontSize": 10 });
            dataArray.push({ text: value['Description'].toString(), "fontSize": 10 });
            dataArray.push({ text: value['UnitPriceString'].toString(), "fontSize": 10 });
            dataArray.push({ text: value['TotalPriceString'].toString(), "fontSize": 10 });

            docDefinition.content[7].table.body.push(dataArray);
        }

        if (data.ContentOwnerES !== null)
            docDefinition.images.ContentOwner = data.ContentOwnerES;

        if (data.ApprovedES !== null)
            docDefinition.images.AuthorizedPerson = data.ApprovedES;

        if (data.IsCancelled !== true) {
            docDefinition.watermark.text = data.OrderNumber;
            docDefinition.watermark.color = 'white';
            docDefinition.watermark.opacity = -1;
        }

        if (data.ScopeItemsCategory === 'Software') {
            var softwareTerms = [[{
                "style": "header",
                "text": "1. Definitions"
            }, {
                "style": "normal",
                "text": "CLIENT means the party purchasing the SOFTWARE, being the legal entity as mentioned in the Purchase Order, as well as his legal successors in title;\nVENDOR means the party supplying the SOFTWARE;\nSOFTWARE means the delivery of software, all documentation belonging thereto and the implementation thereof where required under the CONTRACT on the computer system of CLIENT, all in accordance with the CONTRACT;\nCONTRACT consists of the Purchase Order signed by CLIENT and VENDOR, all listed documents mentioned in the Purchase Order, and any special agreements made between CLIENT and VENDOR."
            }, {
                "style": "header",
                "text": "2. Conflicts"
            }, {
                "style": "normal",
                "text": "Where conflicts occur between or within the contents of the CONTRACT, codes and/or regulations, the most stringent and/or severe requirements for the VENDOR apply. In case of doubt VENDOR shall inform CLIENT in a timely fashion and CLIENT will thereupon indicate the applicable condition."
            }, {
                "style": "header",
                "text": "3. Laws"
            }, {
                "style": "normal",
                "text": "a) VENDOR warrants that all SOFTWARE will comply with all applicable codes, laws and regulations."
            }, {
                "style": "normal",
                "text": "b)\tVENDOR shall indemnify, exonerate and hold CLIENT free and harmless from and against any liability, or penalty imposed upon CLIENT, resulting from each contravention or alleged contravention of the applicable codes, laws and regulations."
            }, {
                "style": "normal",
                "text": "c)\tIn respect of codes and regulations the parties will review applicability on a case-by-case basis."
            }, {
                "style": "header",
                "text": "4. Inclusions"
            }, {
                "style": "normal",
                "text": "All costs of delivery of the SOFTWARE and/or other requirements for supply as laid down in the CONTRACT and as required by applicable laws are included in the contract price, unless specifically stated otherwise in the CONTRACT. Where installation, implementation and other associated services for the SOFTWARE  are required by CLIENT these are specifically identified in the CONTRACT."
            }, {
                "style": "header",
                "text": "5. Changes to CONTRACT"
            }, {
                "style": "normal",
                "text": "Changes will be settled in accordance with the CONTRACT or – if not mentioned therein – will be agreed upon between CLIENT and VENDOR. No substitution or modification by the VENDOR will be permitted except on specific written authority of CLIENT."
            }, {
                "style": "header",
                "text": "6. Expediting"
            }, {
                "style": "normal",
                "text": "VENDOR warrants the timely performance of all of its obligations in accordance with the CONTRACT. If VENDOR encounters delays in obtaining materials and deliverables from his sub-suppliers or in receiving information from"
            },
            {
                "style": "normal",
                "text": " at any time before or within thirty (30) days from the date the SOFTWARE has been placed in use, VENDOR shall at his own expense immediately take all measures to repair, replace or refund the purchase price of the SOFTWARE."
            },
            {
                "style": "normal",
                "text": "b.\tPerformance Warranty.  VENDOR warrants for a period of thirty (30) calendar days from the date of shipment, or date of completion of the acceptance test, if applicable, that the SOFTWARE shall perform in accordance with the documentation supplied with the particular SOFTWARE.  Reference data and Solutionware are provided “as is” and without any warranties whatsoever."
            }, {
                "style": "normal",
                "text": "c.\tSoftware Media Warranty.  VENDOR warrants for a period of thirty (30) calendar days from the date of shipment, or date of completion of the tests required consequent to paragraph 7 above and agreed with VENDOR if applicable, that, under normal use, software delivery media will be free of defects in material and workmanship."
            }, {
                "style": "normal",
                "text": "d.\tVendor does not warrant that the SOFTWARE will meet CLIENT’s requirements, and under no circumstances does Vendor warrant that any Vendor SOFTWARE will operate uninterrupted or error free."
            }, {
                "style": "normal",
                "text": "e.\tVENDOR warrants and represents that it has the right to grant licenses for its SOFTWARE."
            }, {
                "style": "normal",
                "text": "The foregoing warranties are void if failure of a warranted item results, directly or indirectly, from an unauthorized modification to a warranted item; an unauthorized attempt to repair a warranted item; or misuse of a warranted item, including without limitation use of warranted item under abnormal operating conditions or without routinely maintaining a warranted item. CLIENTagrees to notify VENDOR promptly of any suspected defects in software delivery media or this Software Product."
            }, {
                "style": "normal",
                "text": "If under the law ruled applicable to this agreement a greater warranty is mandated, then vendor warrants the software product to the minimum extent required by said law."
            }, {
                "style": "normal",
                "text": "VENDOR’s entire liability and CLIENT’s exclusive remedy shall be, at CLIENT’s sole discretion either.\ni.\t the repair or replacement of any warranted item which does not meet the respective warranties given above, or\nii.\ta refund of the purchase price of the warranted item\nThe above warranties are in lieu of all other warranties, express or implied, and represent the full and total obligation and/or liability of VENDOR."
            }, {
                "style": "normal",
                "text": "Except as provided herein, Vendor makes no representations or warranties with respect to the SOFTWARE express or implied, including the implied warranties of merchantability and fitness for a particular purpose.  If under the law ruled applicable to this agreement any part of the above disclaimer of expressed or implied warranties is invalid, then VENDOR disclaims express or implied warranties to the maximum extent allowed by said law."
            }, {
                "style": "normal",
                "text": " licensing is hereby incorporated into this CONTRACT by reference hereto.",
            }, {
                "style": "header",
                "text": "15. Copies"
            }, {
                "style": "normal",
                "text": "For back-up purposes CLIENT is permitted to reproduce the SOFTWARE in whole or in part."
            }, {
                "style": "header",
                "text": "16. Escrow"
            }, {
                "style": "normal",
                "text": "a)\tOn mutual agreement between the VENDOR and the CLIENT an escrow agreement will be concluded using an escrow party to be designated by VENDOR with regard to SOFTWARE, which is placed at CLIENT’s disposal and of which the intellectual (property) rights are not CLIENT’s.   The VENDOR has standing escrow agreements   and all costs related to extending such escrow agreements for the benefit of the CLIENT shall be for CLIENT’s account."
            }, {
                "style": "normal",
                "text": "c)\tIn the event any Product furnished hereunder is, in VENDOR’s opinion, likely to become the subject of a claim of infringement, or does become the subject of a claim of infringement, of any duly issued Berne Convention countries’ patent, copyright or trademark of a third party, VENDOR at its option and expense may, either procure for CLIENT and its Affiliates the right to continue using the Product, or modify the Product to make it non-infringing but functionally the same, or replace the Product with a non-infringing equivalent."
            }, {
                "style": "header",
                "text": "17. Developments"
            }, {
                "style": "normal",
                "text": "Providing CLIENT has entered into a Software Maintenance Agreement with VENDOR, VENDOR shall promptly inform CLIENT of any new software development and immediately offer to CLIENT updated, enhanced and completely new versions of the SOFTWARE, for a price as stated in the CONTRACT, or such other price to be mutually agreed upon. Use of the original version of the SOFTWARE remains permitted."
            }, {
                "style": "header",
                "text": "18. Assignment"
            }, {
                "style": "normal",
                "text": "Neither VENDOR nor CLIENT shall assign any of its rights or delegate any of its obligations under this CONTRACT without the prior written consent of the other, provided that such consent shall not be unreasonably withheld, except that VENDOR may assign its rights and obligations under this CONTRACT without the approval of the CLIENT to an entity which acquires all or substantially all of the assets of the VENDOR or to any subsidiary, affiliate or successor in a merger or acquisition of the VENDOR."
            }, {
                "style": "header",
                "text": "19. Rights of third parties"
            }, {
                "style": "normal",
                "text": "a)\t In the event of any proceeding (suit, claim, or action) against CLIENT, its Affiliates and Approved Contractors arising from allegations that the licensed Software Products and Documentation or part thereof, furnished by VENDOR under this Agreement (hereinafter “Product”) infringes a third party’s patent, copyright, or trademark protected in the member countries of (i) both the Bern Convention and WCT (“WIPO Copyright Treaty”) or (ii) both the Bern Convention and TRIPS Agreement (“The"
            }, {
                "style": "header",
                "text": "22. Termination by CLIENT for other reasons"
            }, {
                "style": "normal",
                "text": "CLIENT may terminate the CONTRACT in whole or in part by written notice to VENDOR. In such event CLIENT shall make payment to and VENDOR shall accept payment of that part of the purchase price(s) that corresponds with the part of the CONTRACT already executed at the date the termination becomes effective."
            }, {
                "style": "header",
                "text": "23. Force Majeure"
            }, {
                "style": "normal",
                "text": "a)\tForce Majeure is defined as an occurrence which is not for the risk of the party affected and which cannot be reasonably foreseen, controlled or prevented and materially affects the execution of the CONTRACT. Reasons like ordinary hazards of shortage of labour or material or transport, rejection of material, strikes other than general strikes, default of sub-suppliers, price or wage increase, etc., shall not be considered as Force Majeure.\nb)\tIf and as far as compliance with the contractual obligations is not possible as a result of Force Majeure, compliance of such obligations shall, provided that the party affected has timely notified the other party and proven such occurrence, be suspended for the duration of the Force Majeure. In that case the corresponding obligation of the other party shall be suspended for the same time.\nc)\tNo financial claims based on Force Majeure against CLIENT and/or VENDOR may be submitted or maintained."
            }, {
                "style": "header",
                "text": "24. Business Conduct "
            }, {
                "style": "header",
                "text": "24.1 Compliance Obligation"
            }, {
                "style": "normal",
                "text": "Vendor shall comply with all applicable laws and regulations including but not limited to anti-corruption, anti-money laundering, anti-terrorism, export control, economic sanction and anti-boycott laws, regulations and administrative requirements applicable to Vendor or its services."
            }, {
                "style": "header",
                "text": "24.2 Anti-Corruption Obligation"
            }, {
                "style": "normal",
                "text": "Vendor hereby represents and warrants that neither payments nor any other advantages or favours have been or shall be, directly or indirectly, offered, promised, or provided to:\n(i)\ta private party; which as a result could lead to an improper advantage in relation to the business of CLIENT  or \n(ii)\ta public official, member of the judicial system or any other government-related or state-owned entity or person (\"Public Official”) for himself or herself or another person or entity, in order to influence official action, or any Public Official"
            }, {
                "style": "header",
                "text": "24.3 Termination Right"
            }, {
                "style": "normal",
                "text": "Vendor acknowledges and agrees that any breach of the Business Conduct Clauses set out in Clause 24 of this Agreement will be deemed a material breach of contract entitling CLIENT to terminate the Agreement at any time and with immediate effect, without any obligation to pay any outstanding fees or make any other payment CLIENT"
            }
            ], [
                {//0
                    "style": "normal",
                    "text": " CLIENT, VENDOR shall immediately advise CLIENT.",
                },
                {//1
                    "style": "header",
                    "text": "7. Tests"
                }, {
                    "style": "normal",
                    "text": "a)\tWhere implementation services have been purchased by the CLIENT, the implementation of the SOFTWARE is completed when VENDOR - in presence of CLIENT - has demonstrated that the implemented SOFTWARE functions are in accordance and comply with the CONTRACT or unless otherwise specified in the CONTRACT, that the SOFTWARE functions substantially in accordance with the documentation supplied with the SOFTWARE. The SOFTWARE is deemed to have been placed in use from the time that the implementation has either been tested successfully according to CLIENT or the CLIENT has made operational use of the SOFTWARE."
                }, {
                    "style": "normal",
                    "text": "b)\tIf implementation by VENDOR is not part of the work to be performed by VENDOR under the CONTRACT, VENDOR agrees that:"
                }, {
                    "style": "normal",
                    "text": "1.\twithin a reasonable time after the supply of the SOFTWARE, CLIENT can perform tests in order to check whether the SOFTWARE complies with the CONTRACT or unless otherwise  specified in the CONTRACT, that the SOFTWARE complies substantially with the documentation supplied with the SOFTWARE;"
                }, {
                    "style": "normal",
                    "text": "2.\tVENDOR - if CLIENT establishes that such compliance does not exist - shall at his own expense promptly either repair, replace the SOFTWARE to ensure said compliance, after which CLIENT can repeat said tests on the same conditions.;"
                }, {
                    "style": "normal",
                    "text": "3.\tfailure to test by CLIENT and/or any other authority designated by CLIENT, within the Guarantee period  shall not relieve VENDOR of responsibility to ensure that  the SOFTWARE  complies with the documentation supplied with the SOFTWARE nor be interpreted in any way to imply acceptance thereof by CLIENT."
                }, {
                    "style": "normal",
                    "text": "c)\tIf the implementation by VENDOR is not part of the work to be performed by VENDOR under the CONTRACT, the SOFTWARE is deemed to have been placed in use at the time of supply, unless it should appear within the Guarantee period that the SOFTWARE does not comply with the CONTRACT or unless otherwise specified in the CONTRACT does not comply with the documentation supplied with the SOFTWARE. In that case the SOFTWARE is deemed to have been placed in use at the time that the SOFTWARE as yet complies with the CONTRACT or unless otherwise specified in the CONTRACT complies with the documentation supplied with the SOFTWARE."
                }, {
                    "style": "header",
                    "text": "8. Guarantees"
                }, {
                    "style": "normal",
                    "text": "a.\tThe SOFTWARE, the implementation and possible corrective work shall comply with reasonably expected industry standards. If the SOFTWARE or any part thereof and/or the implementation and/or the possible corrective work is found not to be in accordance with the CONTRACT"
                }, {
                    "style": "header",
                    "text": "9. Loss or damage"
                }, {
                    "style": "normal",
                    "text": "Up to the time that the SOFTWARE is placed in use, VENDOR retains the risk of loss of and damage to the SOFTWARE or any part thereof. If loss or damage occurs after this time, CLIENT is entitled against payment of the material cost and in case of damage on submission of the damaged SOFTWARE, to obtain a new copy to be provided by VENDOR."
                }, {
                    "style": "header",
                    "text": "10. General liability"
                }, {
                    "style": "normal",
                    "text": "With regard to the services rendered by VENDOR, his sub-suppliers, agents or employees in respect of the supply, implementation, placing in use, instruction, correction, etc., VENDOR agrees to indemnify CLIENT with respect to all damage and/or injury or death resulting from acts of VENDOR, his sub-suppliers, his agents or employees, while on the premises of CLIENT.\nVENDOR indemnifies CLIENT from all claims, suits, actions and proceedings whatsoever, including those on account of damage and/or injury or death on the side of VENDOR, his agents or employees or other persons on account of said acts."
                }, {
                    "style": "header",
                    "text": "11. Passing of ownership"
                }, {
                    "style": "normal",
                    "text": "The ownership of the physical media which contains   SOFTWARE shall at the latest pass to CLIENT at the time of supply. Otherwise the SOFTWARE is a proprietary product of the VENDOR and relevant third parties and is protected by copyright laws and international treaty.  Title to this SOFTWARE or any copy, modification, or merged portion of this SOFTWARE shall at all times remain with VENDOR and such third parties.  The SOFTWARE is licensed, not sold."
                }, {
                    "style": "header",
                    "text": "12. Errors in delivery"
                }, {
                    "style": "normal",
                    "text": "SOFTWARE delivered or made available in error or SOFTWARE deliverables in excess of the quantity called for in the CONTRACT will be returned at VENDOR’s expense and risk."
                }, { //18
                    "style": "header",
                    "text": "13. Payment"
                }, { //19
                    "style": "normal",
                    "text": "VENDOR shall submit a separate invoice for each payment due in accordance with the payment conditions stated in the CONTRACT. Invoices will be paid in accordance with the payment terms included in the Purchase Order, or, in addition to or failing such payment terms, within {0} calendar days computed from the date of VENDOR's fulfillment of the specified conditions and the date of receipt of VENDOR's invoice, provided such invoice is properly drawn and accompanied by the required supporting documents. If invoices and/or supporting documents require correction the time of payment will be computed from the date of receipt of the corrected invoice and/or documents."
                }, {
                    "style": "header",
                    "text": "14. Use of software"
                }, {
                    "style": "normal",
                    "text": "VENDOR warrants that the CLIENT is permitted to use the SOFTWARE in accordance with the VENDOR’s current Software License Agreement which for the purpose of"
                }, {
                    "style": "normal",
                    "text": " Agreement on Trade-Related Aspects of Intellectual Property Rights”), VENDOR shall, if such infringement does not result solely from modifications, enhancements, or additions to the Product made by CLIENT, an Affiliate or Approved Contractors or any person or entity, acting under the direction or control of the CLIENT, its Affiliates or Approved Contractors or CLIENT’s, its Affiliates’ or Approved Contractors’ use of any Product in combination with other products not furnished by VENDOR, and provided CLIENT or its Affiliates promptly notifies VENDOR in writing of said proceedings, defend CLIENT’s, Affiliates’ or Approved Contractors’ right, or interest in the Product, and said infringement claim at VENDOR’s expense and VENDOR shall pay any judgment or settlement against the CLIENT or Affiliates resulting from said proceeding. VENDOR shall make such defence by counsel of its own choosing and CLIENT, its Affiliates and Approved Contractors shall reasonably cooperate with said counsel. VENDOR shall have sole control of said defence and settlement of any such claim."
                }, {
                    "style": "normal",
                    "text": "b)\tIn the event any such infringement is found by a court of competent jurisdiction to be caused by  modifications, enhancements or additions to the Product made by the CLIENT, its Affiliates or its Approved Contractors or any person or entity, acting under the direction or control of the CLIENT, its Affiliates or its Approved Contractors or CLIENT’s, its Affiliates’ or its Approved Contractors’ use of the Product in combination with other products not furnished by VENDOR, CLIENT agrees to reimburse VENDOR any reasonable defence expenses inclusive of reasonable attorney’s fees which may have been expended by VENDOR in defence of said claim, as well as to pay any  judgment rendered against VENDOR as a result of said proceedings.  "
                }, {
                    "style": "header",
                    "text": "20. Confidentiality"
                }, {
                    "style": "normal",
                    "text": "All engineering data, designs, drawings and other documents supplied to the VENDOR by the CLIENT are confidential and shall not be used for any purpose whatsoever other than in connection with the VENDOR’s obligations under the CONTRACT. Also VENDOR shall not get into any of CLIENT’s data, which is not required for the execution of the work. Unauthorized entrance or distribution is a reason for termination of CONTRACT. "
                }, {
                    "style": "header",
                    "text": "21. Immediate termination"
                }, {
                    "style": "normal",
                    "text": "a)\tIf either the VENDOR or the CLIENT commits a material breach of this CONTRACT which is incapable of remedy, then the non breaching party shall issue written notice of immediate termination to the other.  If either the VENDOR or the CLIENT becomes bankrupt or insolvent, goes into liquidation, has a receiving or administration order made against him or if a winding up petition is submitted, that party is also considered to be in material default. In such case the non breaching party may immediately (partially) terminate the CONTRACT.\nCLIENT may at any time terminate the CONTRACT or any part thereof for any reason crucial to CLIENT providing that the CLIENT’s obligations accrued prior to such termination shall remain due."
                }, {
                    "style": "normal",
                    "text": " shall not be obliged to compensate any loss suffered by the Vendor as the result of a termination under this Clause 24.3 (Termination Right)."
                }, {
                    "style": "header",
                    "text": "24.4 Books and Records"
                }, {
                    "style": "normal",
                    "text": "Vendor shall keep full records in relation to the performance of this Agreement. The content of these records shall include, but not be limited to full and accurate description of performance of Vendor and its Subcontractors (e.g. details of service providers, timesheets, and relevant correspondence or summaries thereof), all expenditures, all payments made and any other documents created or received in connection with this Agreement with CLIENT"
                }, {
                    "style": "header",
                    "text": "24.5 Payment Details"
                }, {
                    "style": "normal",
                    "text": "All payments to Vendor by CLIENT will be made only after receipt of an invoice referring to the Agreement, by transfer to a bank account in Vendor’s name in the country where the services are to be provided or where Vendor has established or maintains its principal place of business."
                }, {
                    "style": "header",
                    "text": "24.6 Compliance with Bilfinger Vendor Declaration"
                }, {
                    "style": "normal",
                    "text": "Vendor shall comply with the Bilfinger Vendor Declaration. A current version of Vendor Declaration is available on Bilfinger’s website. The Bilfinger Vendor Declaration sets the minimum standards that must be applied. However, to the extent, the Bilfinger Vendor Declaration conflicts with local law, local law shall apply. Contractor confirms, by signature to this Agreement, that it has endorsed the Vendor declaration required by Bilfinger SE and all its group companies and will abide by the Declaration"
                }, {
                    "style": "header",
                    "text": "25. Publicity"
                }, {
                    "style": "normal",
                    "text": "Without CLIENT’s prior written approval VENDOR shall not make public any details of the CONTRACT and/or CLIENT."
                }, {
                    "style": "header",
                    "text": "26. Disputes"
                }, {
                    "style": "normal",
                    "text": "All disputes arising in connection with the Agreement shall be finally settled by the competent civil court in Muscati, Sultanate Of Oman in accordance with the rules of Muscat Commercial Conciliation and Arbitration Center at the Muscat Chamber of Commerce and Industry. The arbitration proceedings shall be conducted in the English language."
                }, {
                    "style": "header",
                    "text": "27. Governing law"
                }, {
                    "style": "normal",
                    "text": "The Contract shall be construed, interpreted and applied in accordance with the laws of the Sultanate Of Oman, The “United Nations Convention on Contracts for the International Sale of Goods“ (Vienna, 11 April 1980) will not be applicable."
                }, {
                    "style": "header",
                    "text": "28. Language"
                }, {
                    "style": "normal",
                    "text": "All correspondence and documents in connection with the CONTRACT shall be in the English language."
                }
            ]];
            softwareTerms[1][19].text = softwareTerms[1][19].text.replace("{0}", (data.PaymentTerm === null ? '90' : data.PaymentTerm).substring(0, 3));

            docDefinition.content[31].columns = softwareTerms;
        }
        else {
            var generalTerms = [
                [//0
                    {//0
                        text: '1. Definitions',
                        style: 'header'
                    },
                    {//1
                        text: '-  CLIENT means the party placing an order, being the legal entity as mentioned in the Purchase Order, as well as his legal successors in title;\n- VENDOR means the party who delivers GOODS;\n-  ENGINEER means the party representing CLIENT and acting on behalf and for account of CLIENT;\n-  GOODS mean the goods, material and/or equipment to be supplied by VENDOR in accordance with the Contract;\n-  CONTRACT means the documents as specified in clause 2 of these General Conditions of Purchase.',
                        style: 'normal'
                    },
                    {//2
                        text: '2. CONTRACT',
                        style: 'header'
                    },
                    {//3
                        text: '2.1  The CONTRACT consists of the Purchase Order signed by CLIENT and VENDOR, all listed documents mentioned in the Purchase Order, and any special agreements made between CLIENT and VENDOR.',
                        style: 'normal'
                    },
                    {//4
                        text: '2.2  Agreements, oral or in writing, made between VENDOR and un-authorised personnel of CLIENT or ENGINEER will not be binding on CLIENT.',
                        style: 'normal'
                    },
                    {//5
                        text: '2.3  Where conflicts occur between or within the CONTRACT, codes and/or (legal) regulations the most stringent and/or severe requirements for the VENDOR will apply. In case of doubt ENGINEER, if requested, will indicate the applicable condition.',
                        style: 'normal'
                    },
                    {//6
                        text: '2.4  Should any errors or omissions appear in the contract documents, VENDOR shall report the same to ENGINEER for correction before proceeding with the manufacture or delivery of GOODS. If VENDOR fails to do so, any consequences shall be for his account. VENDOR shall abide by and comply with the CONTRACT and their purport, and shall not avail himself of errors or omissions, should any exist, to restrict his obligations.',
                        style: 'normal'
                    },
                    {//7
                        text: '3. Delivery  terms',
                        style: 'header'
                    },
                    {//8
                        text: 'Interpretation of the delivery terms as stipulated in the CONTRACT will be in accordance with “Incoterms”, edition valid on the date of signment of the CONTRACT, as published by the International Chamber of Commerce (ICC).',
                        style: 'normal'
                    },
                    {//9
                        text: '4. Local laws',
                        style: 'header'
                    },
                    {//10
                        text: 'VENDOR warrants that GOODS will comply with all applicable Governmental codes, laws or regulations, local or national of the country in which GOODS are to be used and that he will, prior to the delivery of GOODS, provide ENGINEER with whatsoever Governmental or other authorisation documents and have whatsoever Governmental or other authorisation markings stamped on GOODS required for the use thereof.',
                        style: 'normal',
                        margin: [0, 10, 10, 45]
                    },
                    {//11
                        text: 'e)  such inspection or failure to inspect by ENGINEER and/or CLIENT or any other authority shall not relieve VENDOR of any responsibility or liability with respect to GOODS nor be interpreted in any way to imply acceptance thereof;\nf)  if as a consequence of disapproval or any other cause for which VENDOR is responsible, inspection (in part) has to be repeated or has still to be performed, the extra costs for CLIENT and/or ENGINEER will be for the account of VENDOR.',
                        style: 'normal'
                    },
                    {
                        text: '11. Guarantees',
                        style: 'header'
                    },
                    {
                        text: '11.1  All GOODS furnished shall be new and in accordance with the CONTRACT specifications and shall be of the best quality of their respective kinds incorporating first class workmanship throughout and applying the latest standard of technology. All GOODS shall be of the required size and capacity and be manufactured of proper materials to fulfil in all respects the operating conditions specified. All GOODS shall be free and clear of all liens, security interests and encumbrances.',
                        style: 'normal'
                    },
                    {
                        text: '11.2  The guarantee period of twelve (12) months will commence on the date GOODS are placed in use or operation and taken over by CLIENT, unless the factory guarantee is valid for a longer period, in that case the guarantee period will be the one which has been given by the manufacturer. If any GOODS do not comply with the specifications or are found defective or if any defect or fault originating from the design (if furnished by VENDOR and/or sub-vendor), materials, workmanship or operating characteristics of any GOODS arise at any time before or within the guarantee period, VENDOR shall at his own expense promptly make such alterations, repairs and replacements as necessary so that said item conforms to the specifications to CLIENT’s entire satisfaction. If VENDOR does not make such corrections promptly, CLIENT will make or have made the required alterations, repairs and replacements at VENDOR’s expense. If the fault or failure to function properly cannot be corrected as set forth above, the defective GOODS shall be removed by or at the expense of VENDOR and VENDOR shall without cost to CLIENT promptly furnish a satisfactory item which completely fulfils the specifications and intent of the CONTRACT and/or ENGINEER or, at CLIENT’s and/or ENGINEER’s option, refund the full purchase price and cost of original transport to the point of installation. This provision is without prejudice to any other rights CLIENT may have.',
                        style: 'normal'
                    },
                    {
                        text: '11.3  The above mentioned guarantee shall also extend to cover the altered, repaired, replaced or substituted item from the time it is established to the satisfaction of CLIENT and/or ENGINEER that the item fulfils the specifications.',
                        style: 'normal'
                    },
                    {
                        text: '11.4  VENDOR will supply all maintenance and repair services and spare parts at fair remuneration during a period of ten years after expiration of the guarantee period.',
                        style: 'normal',
                        margin: [0, 10, 10, 55]
                    },
                    {
                        text: '17. Intellectual property',
                        style: 'header'
                    },
                    {
                        text: 'All intellectual property rights, created under the fabrication of the GOODS, the adjustments thereto, extensions thereto and/or relating (technical) information, documents, procedures, tasks, etc. will vest in CLIENT. As far as existing intellectual property rights and/or (technical) information, documents, procedures, etc, are with VENDOR and/or third parties, VENDOR will arrange an irrevocable right to allow free and unrestricted use by and for the benefit of CLIENT.\nVENDOR shall indemnify and/or hold harmless CLIENT and ENGINEER against any action, claim, demand, costs, charges and expenses arising from or incurred by reason of any infringement of trade name and/or other intellectual property rights of third parties in connection with GOODS or parts thereof, including the use of material or equipment and sale of products manufactured with GOODS.\nIn the event of any claim being made or action brought against CLIENT and/or ENGINEER arising out of the matters referred to in this clause VENDOR shall be promptly notified thereof and shall at his own expense conduct all negotiations for the settlement of the same and any litigation that may arise there from. CLIENT and/or ENGINEER shall at the request of VENDOR afford all available assistance for any such purposes. CLIENT and/or ENGINEER shall be reimbursed any expenses incurred in doing so.\nTermination due to default\nIn case of VENDOR’s failure to comply with any provision of the CLIENT may by giving written notice terminate the CONTRACT or a part thereof without further notice of default and without judicial or arbitral intervention  and without cost or penalty to CLIENT. CLIENT shall be entitled in such case to take over wholly or partially the part of the CONTRACT already executed. In that case CLIENT shall pay and VENDOR shall accept payment of costs incurred prior to such termination that may under generally recognised accounting principles be reasonably allocated to the part of the CONTRACT taken over, less any prepayments made and less compensation for damage caused by VENDOR’s default. In addition CLIENT shall be entitled to claims as provided for in the CONTRACT and/or in the applicable rules of law.',
                        style: 'normal',
                    },
                    {
                        text: '18. Termination by CLIENT',
                        style: 'header'
                    },
                    {
                        text: 'CLIENT may terminate the CONTRACT in whole or in part by written notice to VENDOR. In such event CLIENT shall pay and VENDOR shall accept payment of all costs incurred prior to such termination that may under generally recognised accounting principles be reasonably allocated to the execution of the CONTRACT plus a reasonable allowance for overheads and profit for the part of the CONTRACT executed less payments made.\nIn case of termination for cause or for convenience, VENDOR will at CLIENT’s request assign to CLIENT - to the extent required by CLIENT - the sub-contracts to the CONTRACT entered into by VENDOR.',
                        style: 'normal',
                        margin: [0, 10, 10, 45]
                    },
                    {
                        text: "24.2	Anti-Corruption Obligation",
                        style: 'header',
                    },
                    {
                        text: "Vendor hereby represents and warrants that neither payments nor any other advantages or favours have been or shall be, directly or indirectly, offered, promised, or provided to:",
                        style: 'normal',
                    },
                    {
                        text: "(i)	a private party; which as a result could lead to an improper advantage in relation to the business of CLIENT  or ",
                        style: 'normal',
                    },
                    {
                        text: "(ii)	a public official, member of the judicial system or any other government-related or state-owned entity or person (\"Public Official\") for himself or herself or another person or entity, in order to influence official action, or any Public Official  ",
                        style: 'normal',
                    },
                    {
                        text: "24.3	Termination Right",
                        style: 'header',
                    },
                    {
                        text: "Vendor acknowledges and agrees that any breach of the Business Conduct Clauses set out in Clause 24 of this Agreement will be deemed a material breach of contract entitling CLIENT to terminate the Agreement at any time and with immediate effect, without any obligation to pay any outstanding fees or make any other payment CLIENT shall not be obliged to compensate any loss suffered by the Vendor as the result of a termination under this Clause 1.2 (Termination Right).",
                        style: 'normal',
                    },
                    {
                        text: "24.4	Books and Records",
                        style: 'header',
                    },
                    {
                        text: "Vendor shall keep full records in relation to the performance of this Agreement. The content of these records shall include, but not be limited to full and accurate description of performance of Vendor and its Subcontractors (e.g. details of service providers, timesheets, and relevant correspondence or summaries thereof), all expenditures, all payments made and any other documents created or received in connection with this Agreement with CLIENT.",
                        style: 'normal',
                    },
                    {
                        text: "24.5	Payment Details",
                        style: 'header',
                    },
                    {
                        text: "All payments to Vendor by CLIENT will be made only after receipt of an invoice referring to the Agreement, by transfer to a bank account in Vendor’s name in the country where the services are to be provided or where Vendor has established or maintains its principal place of business.",
                        style: 'normal',
                    },
                    {
                        text: "24.6	Compliance with Bilfinger Vendor Declaration",
                        style: 'header',
                    },
                    {
                        text: "VENDOR shall comply with the Bilfinger Vendor Declaration. A current version of Vendor Declaration is available on Bilfinger’s website. The Bilfinger Vendor Declaration sets the minimum standards that must be applied. However, to the extent, the Bilfinger Vendor Declaration conflicts with local law, local law shall apply.",
                        style: 'normal',
                    },
                    {
                        text: "Contractor confirms, by signature to this Agreement, that it has endorsed the Vendor declaration required by Bilfinger SE and all its group companies and will abide by the Declaration",
                        style: 'normal',
                    },

                ],

                [//1
                    {//0
                        text: '5. Prices',
                        style: 'header'
                    },
                    {//1
                        text: 'All costs of labour, material, documentary and/or other (legal) requirements for supply at times and in quantities as laid down in the CONTRACT and as required by applicable codes, laws and regulations are included in the purchase price(s) unless specifically stated otherwise in the CONTRACT.',
                        style: 'normal'
                    },
                    {//2
                        text: '6. Approval of VENDOR’s documents',
                        style: 'header'
                    },
                    {
                        text: 'Drawings, shop drawings and other documents supplied by VENDOR shall be delivered to ENGINEER as required by the CONTRACT. Approval of VENDOR’s documents does not relieve VENDOR from his exclusive responsibility for their accuracy and/or correctness and does not relieve VENDOR from his obligation to comply fully with the CONTRACT.',
                        style: 'normal'
                    },
                    {
                        text: '7. Changes',
                        style: 'header'
                    },
                    {
                        text: 'ENGINEER may at all times request VENDOR to change, increase or decrease (part of the) GOODS. Any consequences will be agreed upon between CLIENT and VENDOR. No substitution or changes by VENDOR will be permitted except after specific written approval by ENGINEER.',
                        style: 'normal'
                    },
                    {
                        text: '8. Expediting',
                        style: 'header'
                    },
                    {//7
                        text: 'VENDOR shall expedite execution of the CONTRACT, including his orders to sub-vendors. If VENDOR encounters delays in obtaining materials from his sub-vendors or in receiving information from CLIENT and/or ENGINEER, VENDOR shall immediately advise ENGINEER in writing. CLIENT and/or ENGINEER reserve the right to visit the (sub-)vendor’s shop(s) to expedite to whatever extent deemed appropriate without releasing VENDOR from his obligations under the CONTRACT.',
                        style: 'normal'
                    },
                    {
                        text: '9. Sub-Orders',
                        style: 'header'
                    },
                    {
                        text: 'The General Conditions for Purchase are to be extended to all sub-orders issued by VENDOR in connection herewith. VENDOR shall supply CLIENT and/or ENGINEER with unpriced copies of his orders to sub-vendors and any requested shipping information including that of sub-vendors’ orders when and as requested.',
                        style: 'normal'
                    },
                    {
                        text: '10. Inspection',
                        style: 'header',
                    },
                    {
                        text: 'VENDOR agrees that\na)  all inspections and tests shall be made as required by the CONTRACT;\nb)  all GOODS furnished hereunder shall be subject to inspection by ENGINEER and/or CLIENT at all reasonable times and places before, during and after manufacture;\nc)  when inspection is required under the CONTRACT, VENDOR shall give ENGINEER at least five (5) working days written advance notice of readiness for inspection;\nd)  it is VENDOR’s obligation to repair and replace without cost or delay anything found defective during inspection;',
                        style: 'normal',
                        margin: [0, 10, 10, 20]
                    },
                    {
                        text: '12. Passing of ownership',
                        style: 'header'
                    },
                    {
                        text: 'The ownership of GOODS shall at the latest pass to CLIENT at the place of delivery as stated in the CONTRACT.',
                        style: 'normal'
                    },
                    {
                        text: '13. Liability',
                        style: 'header'
                    },
                    {//15
                        text: 'VENDOR is liable for and indemnifies CLIENT against all costs and/or damages arising from his whole or partial non-compliance with the CONTRACT. VENDOR’s liability under this clause shall end five (5) years after acceptance of GOODS. This limitation of the duration of the liability does not apply in case of wilful misconduct, gross negligence or in case of damages suffered by CLIENT as a result of VENDOR’s failure to fulfil his obligations under clause 17 of these General Conditions. CLIENT will give written notice to VENDOR of the nature and extent of the damages suffered. VENDOR will reimburse the damages within 30 days after receipt of said notice.',
                        style: 'normal'
                    },
                    {
                        text: '14. Other supplies / errors in delivery',
                        style: 'header'
                    },
                    {
                        text: 'In the event that VENDOR is required to incorporate in or to connect to GOODS to be supplied under the CONTRACT material or equipment which is supplied to VENDOR directly or indirectly by ENGINEER or CLIENT, or if VENDOR is required to hold materials or equipment on behalf of CLIENT, VENDOR shall be responsible for any loss or damage whatsoever of or to the material or equipment supplied to him from the moment it comes into his possession until moment he delivers it to CLIENT or a third party to whom VENDOR has been directed to deliver it.\nGOODS delivered in error or in excess of the quantity called for in the CONTRACT will be returned at VENDOR’s expense (standard commercial plus/minus practice for bulk materials excepted).',
                        style: 'normal'
                    },
                    {
                        text: '15. Payment',
                        style: 'header'
                    },
                    {//19
                        text: 'VENDOR’s invoices will be paid in accordance with the payment terms included in the Purchase Order, or, in addition to or failing such payment terms, within {0} calendar days computed from the date of CONTRACTOR\'s fulfilment of the specified conditions and the date of receipt of CONTRACTOR\'s invoice, provided such invoice is properly drawn and accompanied by the required supporting documents. If invoices and/or supporting documents require correction the time of payment will be computed from the date of receipt of the corrected invoice and/or documents.\nCLIENT is entitled to balance all amounts due to VENDOR under the CONTRACT with amounts due by VENDOR under the CONTRACT.',
                        style: 'normal'
                    },
                    {
                        text: '16. Assignment',
                        style: 'header'
                    },
                    {
                        text: 'VENDOR shall not assign or transfer any of its rights or obligations under the CONTRACT without CLIENT’s prior written approval.',
                        style: 'normal',
                        margin: [0, 10, 10, 55]
                    },
                    {
                        text: '19. Force Majeure',
                        style: 'header'
                    },
                    {
                        text: 'Force Majeure is defined as any occurrence which cannot be reasonably foreseen, controlled and prevented by VENDOR and which materially affects the execution of the CONTRACT. Normal risks such as ordinary hazards of inclement weather, availability of labour or material or transport, rejection of material, strikes other than general strikes, fluctuation of prices or wages, bankruptcy or insolvency of VENDOR, etc. shall not be considered Force Majeure. VENDOR shall notify ENGINEER immediately in writing of an occurrence of Force Majeure. VENDOR claiming an extension of time because of Force Majeure shall have the burden of proof of the existence of a situation of Force Majeure and that the occurrence affects the progress of the execution of the CONTRACT. Extra costs caused by Force Majeure encountered by VENDOR will not be compensated by CLIENT and/or ENGINEER.',
                        style: 'normal'
                    },
                    {
                        text: '20. Confidentiality',
                        style: 'header'
                    },
                    {
                        text: 'All engineering data, designs, drawings and other documents supplied to VENDOR by ENGINEER and/or CLIENT are confidential and shall not be used for any purpose whatsoever other than for the execution of VENDOR’s obligations under the CONTRACT.',
                        style: 'normal'
                    },
                    {
                        text: '21. Publicity',
                        style: 'header'
                    },
                    {
                        text: 'Without CLIENT’s prior written approval VENDOR shall not make public any details of the CONTRACT, GOODS to be supplied or the purpose for which any GOODS to be supplied hereunder are to be used.',
                        style: 'normal'
                    },
                    {
                        text: '22. Disputes',
                        style: 'header'
                    },
                    {
                        text: 'All disputes arising in connection with the Agreement shall be finally settled by the competent civil court in the sultanate of Oman in accordance with the rules of the Sultanate of Oman Chamber of Commerce and Industry. The arbitration proceedings shall be conducted in the English language.',
                        style: 'normal'
                    },
                    {
                        text: '23. Applicable law',
                        style: 'header'
                    },
                    {
                        text: 'The Contract shall be construed, interpreted and applied in accordance with the laws of the Sultanate of Oman. The “United Nations Convention on Contracts for the International Sale of Goods“ (Vienna, 11 April 1980) will not be applicable.',
                        style: 'normal'
                    },
                    {
                        text: "24. Business Conduct",
                        style: 'header',
                    },
                    {
                        text: "24.1	Compliance Obligation",
                        style: 'header',
                    },
                    {
                        text: "Vendor shall comply with all applicable laws and regulations including but not limited to anti-corruption, anti-money laundering, anti-terrorism, export control, economic sanction and anti-boycott laws, regulations and administrative requirements applicable to Vendor or its services.",
                        style: 'normal',
                        margin: [0, 10, 10, 30]
                    },
                    {
                        text: '25. ENGINEER’s rights',
                        style: 'header'
                    },
                    {
                        text: 'CLIENT stipulates for the benefit of ENGINEER that ENGINEER has the same rights against VENDOR as CLIENT has under the CONTRACT, which stipulation VENDOR accepts.',
                        style: 'normal'
                    },
                    {
                        text: '26. Language',
                        style: 'header'
                    },
                    {
                        text: 'All correspondence and documents in connection with the CONTRACT shall be in the English language.',
                        style: 'normal'
                    }
                ]
            ];

            generalTerms[1][19].text = generalTerms[1][19].text.replace("{0}", (data.PaymentTerm === null ? '90' : data.PaymentTerm).substring(0, 3));

            docDefinition.content[31].columns = generalTerms;
        }

        //console.log(JSON.stringify(docDefinition));
        pdfMake.createPdf(docDefinition).download("Order " + data.OrderNumber + " .pdf");
    });
}