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
                        { text: 'Tebodin & Partners LLC,', fontSize: 7, margin: [50, 20, 0, 0] },
                        { text: 'P.O. Box 716, Postal Code 130, Al- Azaibah,', fontSize: 7, margin: [50, 0] },
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
                text: "The supplier is obliged to comply with the Tebodin / Bilfinger Vendor Declaration.",
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
            },
            {//17
                columns: [
                    { text: '', "fontSize": 10, width: 50 },
                    { text: 'This Order Confirmation shall consist of the Section I of Commercial Terms and Section II of General Terms E510.362.012 rev 01 February 2016, In the event of any conflict between these Sections, the order of precedence shall be as listed above.', "fontSize": 10 },
                ],
                margin: [10, 10, 0, 0]
            },
            {//18
                text: "SECTION I – Commercial Terms",
                "fontSize": 12,
                bold: true,
                margin: [10, 30, 0, 0],
                pageBreak: 'before'
            },
            {//19
                text: "All invoices shall be submitted in one (1) Original hardcopy and one (1) electronic copy.",
                "fontSize": 10,
                margin: [10, 30, 0, 0],
            },
            {//20
                text: "1.	Delivery Destination:",
                "fontSize": 10,
                margin: [10, 10, 0, 0],
            },
            {//21
                text: "The goods shall be delivered to the following address:",
                "fontSize": 10,
                margin: [10, 10, 0, 0],
            },
            {//22
                text: "2nd Floor, Azaiba Plaza, Building no:187, Way 61 Al-Ma'aridh St. Ghala. Sultanate of Oman",
                "fontSize": 10,
                margin: [10, 10, 0, 0],
            },
            {//23
                text: "2.	Invoicing\nTebodin & Partners LLC\n2nd Floor, Azaiba Plaza, Building no:187, Way 61,\nAl-Ma'aridh St. Ghala\nP.O. Box 716,\nPostal Code 130, Al- Azaibah\nSultanate of Oman\nPhone +968 2421 5000",
                "fontSize": 10,
                margin: [10, 10, 0, 0],
            },
            {//24
                text: "An electronic copy of invoices shall be mailed to the Procurement representatives specified under “Dealt with by” on the first page of this Purchase Order",
                "fontSize": 10,
                margin: [10, 10, 0, 0],
            },
            {//25
                text: "All invoices shall contain the following:",
                "fontSize": 10,
                margin: [10, 10, 0, 0],
            },
            {//26
                columns: [
                    { text: 'a.', "fontSize": 10, width: 30 },
                    { text: 'This Agreement Number XXXXX-XXX supplement XX', "fontSize": 10 },
                ],
                margin: [10, 10, 0, 0]
            },
            {//27
                columns: [
                    { text: 'b.', "fontSize": 10, width: 30 },
                    { text: 'Reference to the relevant Section and/or milestone of this Purchase Order', "fontSize": 10 },
                ],
                margin: [10, 10, 0, 0]
            },
            {//28
                columns: [
                    { text: 'c.', "fontSize": 10, width: 30 },
                    { text: 'Total net invoice amount and currency', "fontSize": 10 },
                ],
                margin: [10, 10, 0, 0]
            },
            {//29
                columns: [
                    { text: 'd.', "fontSize": 10, width: 30 },
                    { text: 'For departments: General', "fontSize": 10 },
                ],
                margin: [10, 10, 0, 0]
            },
            {//30
                text: "In order to prevent delays of the payment procedure the acknowledgement copy shall be returned unaltered, dated and signed for acceptance to Tebodin upon receipt",
                "fontSize": 8,
                margin: [10, 30, 0, 0],
                italics: true,
                pageBreak: 'after'
            },
            {//31
                alignment: 'justify',
                columns: [],
                style: 'columnNormal'
            },
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