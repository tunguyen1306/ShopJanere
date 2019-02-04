var fbLoading = function (el) {
    $(el).addClass('loading');
    $(el).append('<div class="loading_icon" style="position: absolute; z-index: 11; left: 50%; top: 50%;margin-top:-25px; display: none"><svg width="50px" height="50px" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid" class="uil-reload"><rect x="0" y="0" width="100" height="100" fill="none" class="bk"></rect><g transform="rotate(11.9999 50 50)"><path d="M50 15A35 35 0 1 0 74.787 25.213" fill="none" stroke="#F47920" stroke-width="12px"></path><path d="M50 0L50 30L66 15L50 0" fill="#F47920"></path><animateTransform attributeName="transform" type="rotate" from="0 50 50" to="360 50 50" dur="1s" repeatCount="indefinite"></animateTransform></g></svg></div><div class="opacity" style="left: 0px; top: 0px; right: 0px; bottom: 0px; position: absolute; background-color: gray; opacity: 0.5; z-index: 10; display: none"></div>');

}
var fbRemoveLoading = function (el) {
    $(el).removeClass('loading');
    $(el).find('.loading_icon').remove();
}
$('.select2').select2();
