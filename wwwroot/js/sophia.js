// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {


    var findDisableElements = function (element) {
        for (var i = 0; i < element.childNodes.length; i++) {
            if (element.childNodes[i].localName == "input" || element.childNodes[i].localName == "textarea") {
                element.childNodes[i].disabled = true;
            }
            if (element.childNodes[i].childNodes.length > 0) {
                findDisableElements(element.childNodes[i])
            }
        }
    }
    var findEnableElements = function (element) {
        for (var i = 0; i < element.childNodes.length; i++) {
            if (element.childNodes[i].localName == "input" || element.childNodes[i].localName == "textarea") {
                element.childNodes[i].disabled = false;
            }
            if (element.childNodes[i].childNodes.length > 0) {
                findEnableElements(element.childNodes[i])
            }
        }
    }


    var disableInput = function () {
        //function disableInput(name) {
        var allDivs = document.getElementById("createQuestion").getElementsByClassName("disableble");
        for (var i = 0; i < allDivs.length; i++) {
            if (allDivs[i].className.includes(this.value)) {
                findEnableElements(allDivs[i]);
            }
            else {
                findDisableElements(allDivs[i]);
            }

        }
    }
    var disableAllDisableble = function () {
        //function disableInput(name) {
        var allDivs = document.getElementById("createQuestion").getElementsByClassName("disableble");
        for (var i = 0; i < allDivs.length; i++) {
            findDisableElements(allDivs[i]);
        }
    }

    var questionType = document.getElementsByClassName("radioInput form-check-input");
    for (var i = 0; i < questionType.length; i++) {
        //questionType[i].onclick = disableInput(questionType[i].value);
        if (questionType[i].localName == "input") {
            questionType[i].addEventListener('change', disableInput, false);
        }        
    }
    disableAllDisableble();

    var activateSelection = function () {
        var questionType = document.getElementsByClassName("radioInput");
        for (var i = 0; i < questionType.length; i++) {
            //questionType[i].onclick = disableInput(questionType[i].value);
            if (questionType[i].checked == true) {
                var name = questionType[i].value;
                var allDivs = document.getElementById("createQuestion").getElementsByClassName("disableble");
                for (var i = 0; i < allDivs.length; i++) {
                    if (allDivs[i].className.includes(name)) {
                        findEnableElements(allDivs[i]);
                    }
                }
            }
        }
    }

    
    activateSelection();
    
    


    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
});


