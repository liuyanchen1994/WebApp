/**
 * Toast 提醒提示
 */
import * as $ from 'jquery';
export class Toast {
    constructor(parameters) {

    }

    static Success(msg: string): void {
        return this.ActionInfo(msg, "success");
    }

    static Error(msg: string): void {
        return this.ActionInfo(msg, "error");
    }

    static Info(msg: string): void {
        return this.ActionInfo(msg, "info");
    }

    static ActionInfo(msg: string, type: string = "success"): void {
        $(".toast").hide();
        var toastTmp: string;
        let divClass: string;
        let msgClass: string;
        switch (type) {
            case "info":
                divClass = "info";
                msgClass = "info";

                break;
            case "error":
                divClass = "danger";
                msgClass = "danger";
                break;
            case "success":
                divClass = "success";
                msgClass = "success";
                break;
            default:
                break;
        }
        toastTmp = `
        <div class="alert alert-${divClass} toast" role="alert">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <p class="text-${msgClass}">
            <strong>${type!}</strong>
            </br>
            ${msg}
            </p>

        </div>
        `;

        $("body").append(toastTmp);
        setTimeout(function () {
            $(".toast").slideUp(1000);
        }, 3000);
        // console.log(toastTmp);
    }
}