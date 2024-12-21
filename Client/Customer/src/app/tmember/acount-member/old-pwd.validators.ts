
import { AbstractControl, ValidationErrors } from '@angular/forms';

export class OldPwdValidators {
    static matchPwds(control: AbstractControl) {
        let newPwd2 = control.get('passwordNew');
        let confirmPwd2 = control.get('confirmPasswordNew');
        if (newPwd2?.value !== confirmPwd2?.value) {
            return { pwdsDontMatch: true };
        }
        return null;
    }
}