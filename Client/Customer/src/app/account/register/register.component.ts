import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LocationService } from 'src/app/service/location.service';
import { AccountService } from '../account.service';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  hide = true;
  registerFormGroup!: FormGroup;
  resProvince: any;
  provinces: any;
  resDistricts: any;
  districts: any;
  resWards: any;
  wards: any;
  constructor(private dialog: MatDialogRef<RegisterComponent>, private service: AccountService, private formBuilder: FormBuilder, private toastr: ToastrService, private router: Router, private location: LocationService, private dialogO: MatDialog) {

  }
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.registerFormGroup = this.formBuilder.group({
      userFullName: this.formBuilder.control('', Validators.required),
      userID: this.formBuilder.control('', Validators.required),
      password: this.formBuilder.control('', Validators.required),
      email: this.formBuilder.control('', Validators.compose([Validators.email, Validators.required])),
      phoneNumber: this.formBuilder.control('', Validators.compose([Validators.pattern("^[0-9]*$"), Validators.minLength(10), Validators.maxLength(10), Validators.required])),
      gender: this.formBuilder.control('0'),
      address: this.formBuilder.group({
        provinceCode: this.formBuilder.control(''), // Tạo FormControl cho tỉnh
        districtCode: this.formBuilder.control(''), // Tạo FormControl cho huyện
        wardCode: this.formBuilder.control(''),    // Tạo FormControl cho xã
        descriptions: this.formBuilder.control('')
      })
    });
    this.location.getProvinces().subscribe(res => {
      this.resProvince = res;
      this.provinces = this.resProvince.response.data.provinces;
    });
  }

  onProvinceChange() {
    if (this.registerFormGroup.get('address.provinceCode')?.value) {
      this.location.getDistricts(this.registerFormGroup.get('address.provinceCode')?.value).subscribe(res => {
        this.resDistricts = res;
        this.districts = this.resDistricts.response.data.districts;
      });
    }
  }

  onDistrictChange() {
    if (this.registerFormGroup.get('address.districtCode')?.value) {
      this.location.getWards(this.registerFormGroup.get('address.districtCode')?.value).subscribe(res => {
        this.resWards = res;
        this.wards = this.resWards.response.data.wards;
      });
    }
  }

  response: any;
  async processedRegister() {
    if (this.registerFormGroup.valid) {
      try {
        const res = await this.service.reisterProcessService(this.registerFormGroup.value).subscribe(res => {
          this.response = res;
          if (this.response.code === 200 && this.response.response && this.response.response.success === false) {
            this.toastr.error("Registration failed. Account already exists.");
          } else {
            this.toastr.success("register success");
            const dialogConfig = new MatDialogConfig();
            this.dialogO.open(LoginComponent, dialogConfig);
            this.dialog.close();
          }
        }, (error) => {
          console.log(error);
          this.toastr.error("Can't connect to server.");
        });

      } catch (error) {
        console.log(error)
        this.toastr.error('Register fail: ' + error, 'Fail');
      }
    }
    else {
      if (this.registerFormGroup.get('email')?.invalid) {
        this.toastr.warning('Bạn chưa nhập email', 'Error');
      } else if (this.registerFormGroup.get('userFullName')?.invalid) {
        this.toastr.warning('Bạn chưa nhập họ tên', 'Error');
      } else if (this.registerFormGroup.get('userID')?.invalid) {
        this.toastr.warning('Bạn chưa nhập tài khoản', 'Error');
      } else if (this.registerFormGroup.get('password')?.invalid) {
        this.toastr.warning('Bạn chưa nhập mật khẩu', 'Error');
      }
      else if (this.registerFormGroup.get('phoneNumber')?.invalid) {
        this.toastr.warning('Số điện thoại phải được nhập và là số có 10 chữ số', 'Error');
      }

    }
  }
}
