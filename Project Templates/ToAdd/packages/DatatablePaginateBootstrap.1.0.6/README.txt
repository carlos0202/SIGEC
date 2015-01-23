This is an extension script to jQuery Datatables.  It allows you to override the default pager for one with a textbox for entering page numbers.  It also implements a bootstrap highlight and glyphicons.  It is customizable and intuitive.

********************
Standard Usage:
********************

Add the script to the page.  You can add it directly or use a bundle.  It must appear below the script reference to DataTables itself:
<script type="text/javascript" src="~/Scripts/bootstrapPager.1.0.6.min.js"></script>



In your datatable declaration select this plugin:

$('#tbl').DataTable({
    pagingType: "bootstrapPager",
    ...
});


It's as easy as that. If you want to adjust some settings add the pagerSettings object:

$('#tbl').DataTable({
    pagingType: "bootstrapPager",
    pagerSettings: {
       textboxWidth: 70,
       firstIcon: "",
       previousIcon: "glyphicon glyphicon-arrow-left",
       nextIcon: "glyphicon glyphicon-arrow-right",
       lastIcon: "",
       searchOnEnter: true
    },
    ...
});

The above settings would enlarge the textbox, change the icons from chevrons to arrows, remove the first and last buttons, and make it so searching requires an ENTER press:

A list of all the options:
* textboxWidth
* firstIcon
* previousIcon
* nextIcon
* lastIcon
* searchOnEnter


If you are a Font Awesome fan simply reference the library and use those classes instead:

<link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">


<script>
$('#tbl').DataTable({
    pagingType: "bootstrapPager",
    pagerSettings: {
       firstIcon: "fa fa-angle-double-left fa-2x",
       previousIcon: "fa fa-angle-left fa-2x",
       nextIcon: "fa fa-angle-right fa-2x",
       lastIcon: "fa  fa-angle-double-right fa-2x"
    },
    ...
});
</script>