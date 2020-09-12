//var colN = 0;
//var l = 0;

function downloadBidSummary(id, rUrl) {
    var urlProject = rUrl + 'getBidSummaryJSON';
    var urlValue = urlProject + "?id=" + id;
    colN = 0;
    l = 0;
    $.ajax({
        async: true,
        type: "GET",
        cache: false,
        url: urlValue
    }).done(function (data, textStatus, jqXHR) {
        var MaxLength = 0;

        var HeaderMaxLength = 0;
        //Get Header max length
        for (var h = 0; h < 9; h++) {
            if (data.Table[h].length > HeaderMaxLength)
                HeaderMaxLength = data.Table[h].length;
        }

        var rowsHeader = [];
        //Get Header Rows
        for (var r = 0; r < 9; r++) {
            rowsHeader[rowsHeader.length] = data.Table[r];
        }

        //Get max row length from rows list
        for (var l = 9; l < data.Table.length; l++) {
            if (data.Table[l].length > MaxLength)
                MaxLength = data.Table[l].length;
        }

        //add empty cells
        for (var i = 9 ; i < data.Table.length; i++)
            for (var j = 0; j < MaxLength; j++) {
                if (data.Table[i][j] === undefined || data.Table[i][j] === null)
                    data.Table[i][j] = "";
            }

        var rows = [];

        for (var k = 9; k < data.Table.length; k++) {
            rows[rows.length] = data.Table[k];
        }

        var columns = [];
        for (var col = 0; col < MaxLength; col++)
            columns[columns.length] = "";
        var IsTooBig = data.CompanyCount > 4;

        var docDefinition = {
            header: function (currentPage, pageCount) {
                var headerObj = {
                    "canvas": [{
                        "type": "line",
                        "x1": 75,
                        "y1": 75,
                        "x2": IsTooBig ? 2300 : 1609,
                        "y2": 75,
                        "lineWidth": 1
                    }]
                };

                if (currentPage !== 1 && pageCount > 1)
                    return headerObj;
            },
            //footer: function(currentPage, pageCount) {
            //    	var footerObj = {
            //        "canvas": [{
            //    				"type": "line",
            //    				"x1": 75,
            //    				"y1": -11,
            //    				"x2": 1609,
            //                    "y2": -11,
            //                    "lineWidth": 1
            //            }]};
            //    if(currentPage != pageCount && pageCount > 1)
            //    return footerObj;
            //},
            pageSize: IsTooBig ? 'A1' : 'A2',
            pageMargins: [75, 75],
            pageOrientation: 'landscape',
            content: [
                 {//0
                     image: 'bilfinger',
                     width: 90,
                     alignment: 'left',
                     margin: [0, 0, 20, 0]
                 },

                rowsHeader[0],
                rowsHeader[1],
                rowsHeader[2],
                rowsHeader[3],
                rowsHeader[4],
                rowsHeader[5],
                {//7
                    text: rowsHeader[6],
                    style: 'header',
                    alignment: 'center',
                },
                {//8
                    text: rowsHeader[7],
                    bold: true,
                    alignment: 'center',
                },
                {//9
                    text: rowsHeader[8],
                    bold: true,
                    alignment: 'center',
                    "margin": [0, 0, 0, 10]
                },
                {//10
                    "margin": [0, 0, 0, 30],
		            table:
                    {
                        widths: [],
                        heights: 15,
                        body:
                            rows,
                    },
		            layout: {
		                hLineWidth: function (i, node) {
		                    return (i === 0 || i === node.table.body.length) ? 2 : 1;
		                },
		                vLineWidth: function (i, node) {
		                    return (i === 0 || i === node.table.widths.length || (i + 3) % 4 === 0) ? 2 : 1;
		                },
		                hLineColor: function (i, node) {
		                    return (i === 0 || i === node.table.body.length || i === node.table.body.length - 2) ? 'black' : 'gray';
		                },
		                vLineColor: function (i, node) {
		                    return (i === 0 || i === node.table.widths.length || (i + 3) % 4 === 0) ? 'black' : 'gray';
		                },
		                defaultBorder: true,
		            }
		        },
            ],
            images: {
                bilfinger: 'data.Table:image/png;base64,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',
                defaultImage: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8Xw8AAoMBgDTD2qgAAAAASUVORK5CYII='
            },

            styles: {
                header: {
                    fontSize: 18,
                    bold: true,
                    alignment: 'justify',
                },
                grayColour: {
                    fillColor: '#dedede',
                }
            }
        };

        docDefinition.content[10].table.widths = data.ColumnWidths;

        var SignatureRow = [
            {//0
                columns: [
                    { text: 'Name', "fontSize": 10, bold: true }
                ],
                margin: [10, 20, 0, 0]
            },
            {//1
                columns: [
                    { image: 'defaultImage', fit: [80, 100], margin: [0, 0, 0, -100] }
                ],
                margin: [10, 30, 0, 0]
            },
            {//2
                columns: [
                    { text: '_______________________', "fontSize": 10 }
                ],
                margin: [10, 30, 0, 0]
            },
            {//3
                columns: [
                    { text: '', "fontSize": 10, bold: true }

                ],
                margin: [10, 10, 0, 0]
            },
            {//4
                columns: [
                    { text: 'Date', "fontSize": 10, bold: true }
                ],
                margin: [10, 10, 0, 120]
            }
        ];
        

        var srow = {
            columns: []
        };

        for (var i in data.Reviewers) {
            if (parseInt(i) % 4 === 0) {
                docDefinition.content.push(JSON.parse(JSON.stringify(srow)));
            }

            var contentPosition = docDefinition.content.length - 1;
            //console.log(contentPosition + ' - ' + parseInt(i) + ' - ' + ((parseInt(i) + 1) % 4));

            var reviewer = data.Reviewers[i];

            var reviewerSignatureRow = JSON.parse(JSON.stringify(SignatureRow)); //Clone template

            reviewerSignatureRow[0].columns[0].text = reviewer.Name;

            if (reviewer.Signature !== null) {
                docDefinition.images[reviewer.EmployeeNumber] = reviewer.Signature;
                reviewerSignatureRow[1].columns[0].image = reviewer.EmployeeNumber;
            } else {
                reviewerSignatureRow[1].columns[0].image = 'defaultImage';
            }

            reviewerSignatureRow[3].columns[0].text = reviewer.Position;

            reviewerSignatureRow[4].columns[0].text = "Date: " + (reviewer.Date === null ? "-" : reviewer.Date);

            docDefinition.content[contentPosition].columns.push(reviewerSignatureRow);
        }

        //console.log(JSON.stringify(docDefinition));
        //console.log(JSON.stringify(data));
        pdfMake.createPdf(docDefinition).download("BidSummary.pdf");
    });
}