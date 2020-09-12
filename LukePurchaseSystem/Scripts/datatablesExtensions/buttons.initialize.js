    //initialize Constructor for Button Group

    new $.fn.dataTable.Buttons( table, {
        buttons: true,
        "buttons": [
            {
                extend: 'colvis',
                postfixButtons: ['colvisRestore'],
            },
        	{
        	    extend: 'copyHtml5',
        	    exportOptions: {
        	        columns: ':visible'
        	    },
        	    footer: false
        	},
			{
			    extend: 'excelHtml5',
			    exportOptions: {
                    columns: ':visible',
                    modifier: {
                        selected: null
                    },
                },           
			    footer: false
			},
            {
                extend: 'print',
                exportOptions: {
                    columns: ':visible',
                    modifier: {
                        selected: null
                    },
                },
                footer: false
            },
            {
                text: 'Reload',
                action: function (e, dt, node, config) {
                    dt.ajax.reload();
                }
            }
        ],
    });

function getButtonSelector() {
    return typeof(IndexConfig) === 'undefined' ? '.col-sm-6:eq(0)' : IndexConfig.ButtonSelector;
}
    // Attach Buttons
    table.buttons().container()
        .appendTo('#dtTable_wrapper ' + getButtonSelector());

