document.addEventListener("DOMContentLoaded", function () {

    var changeArrangement = function () {
        var console = document.getElementById("console");
        console.innerHTML = "";
        var spiner = document.getElementById("spiner");
        spiner.display = "inline-block";
        var trsHtmlCollection = document.getElementsByClassName("ArrangeItem");
        const ids = [];
        for (var i = 0; i < trsHtmlCollection.length; i++) {
            ids.push(trsHtmlCollection[i].dataset.qid);
        }
        var table = document.getElementsByClassName("table");
        var tsid = table[0].dataset.surveyid;

        (async () => {
            range = {
                sid: tsid,
                qids: ids                
            }
            const data = JSON.stringify(range);
            const rawResponse = await fetch(location.protocol + "//" + location.host + "/Sophia/PytaniaApi/RearrangeSurvey", {                
                headers: {                   
                    'Content-Type': 'application/json'
                },
                method: 'POST',
                body: data
            });
           
            const content = await rawResponse.json();            
            console.innerHTML = content;            
        })();


    }

    var activateUpDownButtons = function () {
        var buttonsDown = document.getElementsByClassName("rearrangeDown")
        for (var i = 0; i < buttonsDown.length; i++) {
            buttonsDown[i].addEventListener("click", moveRowDown)
        }
        var buttonsUp = document.getElementsByClassName("rearrangeUp")
        for (var e = 0; e < buttonsUp.length; e++) {
            buttonsUp[e].addEventListener("click", moveRowUp)
        }
    }

    var moveRowDown = function () {
        var trsHtmlCollection = document.getElementsByClassName("ArrangeItem");
        var trs = Array.from(trsHtmlCollection);
        var tbody = trs[0].parentElement;
        var tr = this.parentElement.parentElement;
        var currentIndex = trs.findIndex((element) => element.dataset.qid == tr.dataset.qid);
        //var filteredElements = trs.filter(function (item, index) { return item.id == removeId; });

        if (currentIndex < trs.length - 1) {
            var belowItem = trs.at(currentIndex + 1);
            tbody.insertBefore(belowItem, tr);
        }
    }
    var moveRowUp = function () {
        var trsHtmlCollection = document.getElementsByClassName("ArrangeItem");
        var trs = Array.from(trsHtmlCollection);
        var tbody = trs[0].parentElement;
        var tr = this.parentElement.parentElement;
        var currentIndex = trs.findIndex((element) => element.dataset.qid === tr.dataset.qid);
        //var filteredElements = trs.filter(function (item, index) { return item.id == removeId; });

        if (currentIndex > 0) {
            var aboveItem = trs.at(currentIndex - 1);
            tbody.insertBefore(tr, aboveItem);
        }
    }
    activateUpDownButtons();

    var rearrangeButton = document.getElementById("rearrangeButton");
    rearrangeButton.addEventListener("click", changeArrangement);

});