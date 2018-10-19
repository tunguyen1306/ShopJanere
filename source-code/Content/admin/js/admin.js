jQuery(document).ready(function(){
	jQuery('#adminmenu > li > a').click(function(){
		jQuery(this).next('ul').slideToggle();		
		jQuery(this).find('i').toggleClass('fa-sort-up fa-sort-down');
		return false;		
	});
	jQuery('#cb_select_all').change(function(){  //"select all" change 
		jQuery('.column_check input[type="checkbox"]').prop('checked', jQuery(this).prop("checked")); //change all ".checkbox" checked status
	});
}); 	