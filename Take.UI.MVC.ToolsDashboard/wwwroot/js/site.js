//################################## NOTIFICATION ##################################
function ShowNotification(type, mensagem, timeClose) {
    
    //Criar elemento toast
    var notification_toast = document.createElement('div');
    $(notification_toast).addClass('notification-toast');

    //Criar o elemento content
    var notification_content = document.createElement('div');
    $(notification_content).addClass('notification-content');

    //Cria o link para fechar a mensagem
    var notification_a_close = document.createElement('a');
    $(notification_a_close).attr("id", "notification-close");
    $(notification_a_close).attr("onclick", "CloseNotification(this);");
    $(notification_a_close).addClass('notification-close');
    $(notification_a_close).addClass('glyphicon glyphicon-remove');

    //seta a classe para o tipo de mensagem
    if (type === 'error') {
        $(notification_content).addClass('notification-content-error');
    }
    else if (type === 'success') {
        $(notification_content).addClass('notification-content-success');
    }

    //Criar o elemento que vai conter a mensagem
    var notification_text = document.createElement('div');
    $(notification_text).append(mensagem);
    $(notification_text).addClass('notification-text');


    //Append os elementos criando a arvore do HTML
    $(notification_text).append(notification_a_close);
    $(notification_content).append(notification_text);
    $(notification_toast).append(notification_content);

    //Adiociona a mensagem na body do HTML
    $('body').append(notification_toast);

    //se o timeClose for informado
    if (timeClose !== null && timeClose !== 0) {

        //Fecha a mensage no tempo desejado
        setTimeout(function () {
            $(notification_toast).addClass("notification-toast-hide");

            setTimeout(function () {
                $(notification_toast).remove();
            }, 500);
        }, timeClose * 1000);
    }
}

//fecha todas as notificações
function CloseAllNotification() {
    $(".notification-toast").each(function () {
        $(this).addClass("notification-toast-hide");

        setTimeout(function () {
            $(this).remove();
        }, 500);
    });
}

//Fecha a notificação quando se clicar no botão de fechar notificação
function CloseNotification(element) {
    $(element).parent().parent().parent().addClass("notification-toast-hide");

    setTimeout(function () {
        $(element).parent().parent().parent().remove();
    }, 500);
}

function SetActiveLink(element) {
    $(element).addClass("active");

    if ($(element).parent().hasClass("dropdown-container")) {
        $(element).parent().css("display", "block");
    }

}
//################################## END NOTIFICATION ##################################