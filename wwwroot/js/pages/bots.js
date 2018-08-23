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
                alert(data);
            }
        }
    );
}

function showError(msg) {
    $('#notificationHeader').text(msg);
    $('#notificationHeader').slideDown();
}

//function setupRecurrence(recurrence) {

//    if (recurrence == "daily") {
//        $('#subSelections').empty().append("<div>Every <input class='form-control input-sm' id='recurDays' type='text'> Days</div>")
//    }
//    else if (recurrence == "weekly") {
//        $('#subSelections').empty().append("<div>Every <input class='form-control input-sm' id='recurWeeks' type='text'> Days</div>")
//    }

//}

