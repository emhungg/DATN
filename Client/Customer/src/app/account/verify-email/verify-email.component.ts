import { HeaderService } from '../../service/header.service';
import { Component, ElementRef, QueryList, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.css']
})
export class VerifyEmailComponent {
  @ViewChildren('inputField')
  inputFields!: QueryList<ElementRef>;
  constructor(private router: Router, private service: AccountService, private formBuilder: FormBuilder, private toastr: ToastrService, private headerService: HeaderService) {

  }
  verificationForm!: FormGroup;
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.verificationForm = this.formBuilder.group({
      v1: this.formBuilder.control('', Validators.required),
      v2: this.formBuilder.control('', Validators.required),
      v3: this.formBuilder.control('', Validators.required),
      v4: this.formBuilder.control('', Validators.required),
      v5: this.formBuilder.control('', Validators.required),
      v6: this.formBuilder.control('', Validators.required)
    });
    this.headerService.hideHeader();
    this.headerService.hideFooter();
  }


  code: any;
  async processValidate() {
    if (this.verificationForm.valid) {
      this.code = `${this.verificationForm.value.v1}${this.verificationForm.value.v2}${this.verificationForm.value.v3}${this.verificationForm.value.v4}${this.verificationForm.value.v5}${this.verificationForm.value.v6}`;
      try {
        await this.service.processValidateCode(this.code).subscribe(res => {
          this.response = res;
          if (this.response.response.success) {
            this.toastr.success("Active account successfuly");
            this.router.navigate(['']);
            sessionStorage.setItem('isActive', 'true');
          } else {
            this.toastr.error("code invalid");
          }

        }, error => {
          console.log(error);
          this.toastr.error("An error while verify code", "Error");
        });
      } catch (error) {
        this.toastr.error("An error while verify code", "Exception");
      }
    } else {
      this.toastr.warning("Invalid code");
    }
  }
  buttonText: string = 'Send code';
  counting: boolean = false;
  response: any;
  async sendCode() {
    try {
      await this.service.sendCodeProcess().subscribe(res => {
        this.response = res;
        if (this.response.response.success) {
          this.toastr.success("A code verify was send to your email")
          this.counting = true;
          this.buttonText = 'Sending...';

          var countdownDuration = 120; // Thời gian countdown (giây)

          const interval = setInterval(() => {
            countdownDuration--;

            if (countdownDuration === 0) {
              clearInterval(interval);
              this.counting = false;
              this.buttonText = 'Send again';
            } else {
              this.buttonText = `${countdownDuration}s`;
            }
          }, 1000);

        } else {
          this.toastr.warning("Can't find your email");
        }
      }, error => {
        console.log(error);
        this.toastr.error("An error while send code", "Error")
      });
    } catch (error) {
      console.log(error);
      this.toastr.error("An error while send code", "Exception");
    }
  }

  // next input
  onInput(index: number) {
    const currentControlName = `v${index}`;
    const currentValue = this.verificationForm.get(currentControlName)?.value;

    if (currentValue !== '') {
      const nextIndex = index;
      if (index < 6) {
        this.inputFields.toArray()[nextIndex].nativeElement.focus();
      }
    }
  }
}
