/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	//config.uiColor = '#AADC6E';
    
    config.filebrowserBrowseUrl = "/Content/ckfinder/ckfinder.html";
    config.filebrowserImageBrowseUrl = "/Content/ckfinder/ckfinder.html?type=Images";
    config.filebrowserFlashBrowseUrl = "/Content/ckfinder/ckfinder.html?type=Flash";

    config.filebrowserUploadUrl = "/Content/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files";
    config.filebrowserImageUploadUrl = "/Content/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images";
    config.filebrowserFlashUploadUrl = "/Content/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash";

    config.filebrowserWindowWidth = "1000";
    config.filebrowserWindowHeight = "600";
    

    config.toolbarGroups = [
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		'/',
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		'/',
		{ name: 'tools', groups: ['tools'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'about', groups: ['about'] }
    ];

    config.removeButtons = 'About,Maximize,Form,Scayt,SelectAll,Find,Cut,Save,Blockquote,BidiLtr,Iframe,Flash,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,CreateDiv,BidiRtl,Anchor,HorizontalRule,ShowBlocks,Print,Preview,NewPage,Replace,Paste,Copy,PasteText,PasteFromWord,PageBreak';
};
