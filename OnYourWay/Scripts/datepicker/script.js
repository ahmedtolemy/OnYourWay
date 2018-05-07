/// loading website

$(window).load(function() {
        
    $(".loading").fadeOut(900, function() {
        
    $("body").css("overflow-y", "auto");    

    });
});

/// map click

$("span.marker").click(function(){
    $(".maps").slideToggle();
});

/// remove parents

$('.alerts a i').on("click", function () {
    $(this).parents('.alerts a').fadeOut(100);
});

$('span.remove-img').on("click", function () {
    $(this).parents('.boxing-img').fadeOut(100);
});

$('.click-close').on("click", function () {
    $(this).parents('.table tr').fadeOut(100);
});

$(document).on("click",".kikt", function (c) {
  c.preventDefault;
  $(this).parents('.block-user').fadeOut(100);
});

$(".add-ask").click(function(){
    $("form.ask-form").slideToggle();
});


// Start Accordion

$(document).on('click', '.panel-heading span.clickable', function(e) {
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {
        $this.parents('.panel').find('.panel-body').slideDown();
        $this.addClass('panel-collapsed');
        $this.find('i').removeClass('fa-plus').addClass('fa-minus');
    } else {
        $this.parents('.panel').find('.panel-body').slideUp();
        $this.removeClass('panel-collapsed');
        $this.find('i').removeClass('fa-minus').addClass('fa-plus');
    }
});

$('.fa-minus').on("click", function () {
    $(this).parents('table.table.table-hover').slideUp();
});

// ADD IMAGE
$('.image-uploader').change(function (event){
    $(this).parents('.images-upload-block').append('<div class="uploaded-block"><img src="'+ URL.createObjectURL(event.target.files[0]) +'"><button class="close">&times;</button></div>');
});

// REMOVE IMAGE
$('.images-upload-block').on('click', '.close',function (){
    $(this).parents('.uploaded-block').remove();
});

/// toollips

$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();   
});

$(".teckt").click(function () {
      $(".site-mune").toggleClass("push-right");
});

$("button.click-chose").click(function () {
      $(".add-saving").toggle();
});

$(".botti").click(function () {
      $(".box-body-table").toggle();
});

$("h6.serch-here").click(function () {
      $(".site-mune").toggleClass("open-site");
});


/// add apend on click 

$(".last-title").click(function () {
   $(this).parent().children(".table.table-hover").slideToggle();
   $(this).children("i").toggleClass("fa-plus");
   $(this).children("i").toggleClass("fa-minus");
});


$(".more-tick").click(function(){
    $(this).parent().find(".cat").append($(this).parent().find('.div-cope').html());
});

$(document).on("click",".closed", function (e) {
  e.preventdefault;
  $(this).parents('.form-personly').fadeOut(100);
});





$(".add-select").click(function(){
    $(this).parents(".box-sign").find(".cat").append($(this).parents(".box-sign").find('.div-copy').html());
});

$(document).on("click",".remove-select", function (e) {
  e.preventdefault;
  $(this).parents('.select-user').remove();
});


$('.label-click-1 label, .label-click-2 label').click(function () {
      
     $(this).addClass('active').siblings().removeClass('active');
     
});


/// click chick-box append

$(".demo").html($(".blocks-1").html());

$("#pls-3").change(function() {
    if(this.checked) {
        $(".demo").html($(".blocks-1").html());
    }
});

$("#pls-4").change(function() {
    if(this.checked) {
        $(".demo").html($(".blocks-2").html());
    }
});

$("#pls-2").change(function() {
    if(this.checked) {
        $(".demo").html($(".blocks-3").html());
        $(".label-click-2").fadeOut();
    }
});

$("#pls-1").change(function() {
    if(this.checked) {
        $(".demo").html($(".blocks-1").html());
        $(".label-click-2").fadeIn();
        $('#pls-3').prop('checked', true);
    }
});

$('.demo').on('click', '.chico', function() {
    $(this).parents('.here-chick').find('.box-chicked').toggle();
});
