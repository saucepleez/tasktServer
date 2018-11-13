function loadWorkforce(fadeIn) {
    $.ajax({
        url: '/Bots/RetrieveWorkforce',
        type: 'GET',
        success: function (response) {
            
        

            if (fadeIn) {
                $('#workforce').empty().hide().append(response).fadeIn();

                window.setInterval(function () {
                    loadWorkforce(false);
                }, 5000);

              
            }
            else {
                $('#workforce').empty().append(response);
            }

          
        },
        error: function (error) {
 
            showError(error.statusText);
        
        }
    });

}
function loadAssignments(fadeIn) {
    $.ajax({
        url: '/Bots/RetrieveAssignments',
        type: 'GET',
        success: function (response) {

            if (fadeIn) {
                $('#assignments').empty().hide().append(response).fadeIn();
            }
            else {
                $('#assignments').empty().append(response);
            }
         
        },
        error: function (error) {
            showError(error.statusText);
        }
    });

}
function loadSchedule(fadeIn) {
    $.ajax({
        url: '/Bots/RetrieveWorkforceSchedule',
        type: 'GET',
        success: function (response) {

            if (fadeIn) {
                $('#schedule').empty().hide().append(response).fadeIn();
            }
            else {
                $('#schedule').empty().append(response);
            }

        },
        error: function (error) {
            showError(error.statusText);
        }
    });

}


function updateRobot(ele, id, approvalAction) {

    $(ele).prop('disabled', true); //disable
    $(ele).parent().empty().append("<div style='color: #10668B' class='la-square-spin la-custom'><div></div></div>")
    $.ajax({
        url: '/Bots/UpdateRobot',
        data: { id: id, approvalAction: approvalAction},
        type: 'GET',
        success: function (response) {
            $('#workforce').empty().append(response);
        },
        error: function (error) {
            showError(error.statusText);
        }
    });
}
function deleteWorkerEntry(key) {
    $.ajax({
        url: '/Bots/DeleteWorkerEntry',
        data: { key: key },
        type: 'POST',
        success: function (response) {

            $('#workforce').empty().append(response);

        },
        error: function (error) {
            showError(error.statusText);
        }
    });

}
function pingWorker(key) {
    $.ajax({
        url: '/Bots/PingWorker',
        data: { key: key },
        type: 'POST',
        success: function (response) {
            alert(response);
        },
        error: function (error) {
            showError(error.statusText);
        }
    });

}
function executeTask(inputID) {

        var input = document.getElementById(inputId);
        var files = input.files;
        var formData = new FormData();

        for (var i = 0; i != files.length; i++) {
            formData.append("files", files[i]);
        }

        $.ajax(
            {
                url: "/UploadAndExecute",
                data: formData,
                processData: false,
                contentType: false,
                type: "POST",
                success: function (data) {
                    alert("Files Uploaded!");
                }
            }
        );

}

function debugExecute(key) {
    $.ajax(
        {
            url: "/Bots/ExecuteTask",
            data: {key: key},
            type: "POST",
            success: function (data) {
                //alert(data);
            }
        }
    );
}

function showError(msg) {
    $('#notificationHeader').text(msg);
    $('#notificationHeader').slideDown();
}

function getLogsContinously(clientKey) {

    if ($('#logs').is(":visible")) {
        $('#logs').hide();
        return;
    }


    $('#logs').show();
    $('#logs').empty();
    window.setInterval(function () {
        getEngineLogs(clientKey);
    }, 500);

}

function getEngineLogs(clientKey) {


    $.ajax(
        {
            url: "/Api/GetLogs",
            data: { ClientName: clientKey },
            type: "GET",
            success: function (data) {

                //get length of logs
                var logLength = $('.engine-log').length;
   
                if (data.length < logLength) {
                   // $('#logs').empty();
                    logLength = 0;
                }

                //check log lengths
                if (logLength == 0) {

                    //no logs have been added to view yet

                    $.each(data, function (key, value) {
                        $('#logs').append('<div id="log' + key + '" class="engine-log">' + value.loggedOn + " - " + value.message + '</div>');
                    });

                    //scroll
                    var objDiv = document.getElementById("logs");
                    objDiv.scrollTop = objDiv.scrollHeight;

                }
                else if (data.length > logLength) {

                    //get difference
                    var difference = data.length - logLength;


                    //append each new log
                    for (var i = data.length - difference; i < data.length; i++) {
                        $('#logs').append('<div id="log' + i + '" class="engine-log">' + data[i].loggedOn + " - " + data[i].message + '</div>');
                        $('#log' + i).delay(250).fadeOut().fadeIn('slow') 
                    }

                    //scroll
                    var objDiv = document.getElementById("logs");
                    objDiv.scrollTop = objDiv.scrollHeight;
                }
               
           









            }
        }
    );

}

//function setupRecurrence(recurrence) {

//    if (recurrence == "daily") {
//        $('#subSelections').empty().append("<div>Every <input class='form-control input-sm' id='recurDays' type='text'> Days</div>")
//    }
//    else if (recurrence == "weekly") {
//        $('#subSelections').empty().append("<div>Every <input class='form-control input-sm' id='recurWeeks' type='text'> Days</div>")
//    }

//}

