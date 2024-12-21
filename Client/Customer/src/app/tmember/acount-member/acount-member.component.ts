import { Component } from '@angular/core';
import { TmemberService } from '../tmember.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OldPwdValidators } from './old-pwd.validators';
import { LocationService } from 'src/app/service/location.service';

@Component({
  selector: 'app-acount-member',
  templateUrl: './acount-member.component.html',
  styleUrls: ['./acount-member.component.css']
})
export class AcountMemberComponent {

  userInfor: any;
  formChangeNG!: FormGroup;
  formChangPassword!: FormGroup;
  formChangeAddress!: FormGroup;
  provinces: any;
  constructor(private service: TmemberService, private spinner: NgxSpinnerService, private toastr: ToastrService, private formBuilder: FormBuilder, private locationService: LocationService) {

  }

  //#region start form
  // Biến để kiểm soát hiển thị/ẩn form đổi mật khẩu và form ban đầu
  isChangePasswordFormVisible: boolean = false;
  isInitialFormVisible: boolean = true;
  isUpdateAddressFormVisible: boolean = false;

  // Hàm để hiển thị form đổi mật khẩu và ẩn form ban đầu
  showChangePasswordForm() {
    this.isChangePasswordFormVisible = true;
    this.isInitialFormVisible = false;
    this.isUpdateAddressFormVisible = false;
  }

  // Hàm để ẩn form đổi mật khẩu và hiện lại form ban đầu
  showInitialForm() {
    this.isChangePasswordFormVisible = false;
    this.isInitialFormVisible = true;
    this.isUpdateAddressFormVisible = false;
  }

  showUpdateAddressForm() {
    this.isChangePasswordFormVisible = false;
    this.isInitialFormVisible = false;
    this.isUpdateAddressFormVisible = true;
    this.locationService.getProvinces().subscribe(res => {
      this.provinces = res.response.data.provinces;
    })
    this.onProvinceChange();
    this.onDistrictChange()
  }
  //#endregion

  ngOnInit() {
    this.getUser();
    this.formChangeNG = this.formBuilder.group({
      name: this.formBuilder.control('', Validators.required),
      gender: this.formBuilder.control('', Validators.required)
    });
    this.formChangPassword = this.formBuilder.group({
      'passwordOld': this.formBuilder.control('', [Validators.required]),
      'passwordNew': this.formBuilder.control('', [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{8,}')]),
      'confirmPasswordNew': this.formBuilder.control('', Validators.required),
    },
      {
        validator: [OldPwdValidators.matchPwds]
      });
    this.formChangeAddress = this.formBuilder.group({
      provinceCode: '',
      districtCode: '',
      wardCode: '',
      descriptions: this.formBuilder.control('', [Validators.required]),
    })


  }

  getUser() {
    try {
      this.service.getUserById().subscribe(data => {
        this.userInfor = data
        this.service.setUserInfor(data)
        if (this.userInfor) {
          this.formChangeNG.get('name')?.setValue(this.userInfor.userFullName)
          this.formChangeNG.get('gender')?.setValue(this.userInfor.gender === 'Nam' ? 0 : 1)
          this.formChangeAddress.get('provinceCode')?.setValue(this.userInfor.provinceCode)
          this.formChangeAddress.get('districtCode')?.setValue(this.userInfor.districtCode)
          this.formChangeAddress.get('wardCode')?.setValue(this.userInfor.wardCode)
          this.formChangeAddress.get('descriptions')?.setValue(this.userInfor.addressUserDes)
        }
      }, error => {
        console.error(error)
        this.toastr.error(error);
        this.spinner.hide();
      });
    } catch (error) {
      console.log(error);
    }
  }

  changeNameGender() {
    if (this.formChangeNG.valid) {
      this.service.changeNameGender(this.formChangeNG.value).subscribe(res => {
        this.toastr.success('Cập nhật thành công', 'Updated')
      }, error => {
        this.toastr.error('Có lỗi trong quá trình cập nhật vui lòng thử lại', 'ERROR')
      })
    } else {
      this.toastr.error('Nhập tên đầy đủ', 'ERROR')
    }
  }
  //#region change password

  get oldPwd() {
    return this.formChangPassword.get('passwordOld');
  }

  get newPwd() {
    return this.formChangPassword.get('passwordNew');
  }

  get confirmPwd() {
    return this.formChangPassword.get('confirmPasswordNew');
  }


  changePassword() {
    if (this.formChangPassword.valid) {
      console.log(this.formChangPassword.value)
      this.service.changePassword(this.formChangPassword.value).subscribe(res => {
        this.formChangPassword.reset()
        this.toastr.success("Thay đổi mật khẩu thành công", "Noti")
      }, error => {
        this.toastr.error("Có lỗi vui lòng thử lại", "Noti")
      })

    } else {
      this.toastr.warning("Nhập đầy đủ thông tin", "Noti")
    }
  }
  //#endregion

  districts: any;
  onProvinceChange() {
    if (this.formChangeAddress.get('provinceCode')?.value) {
      this.locationService.getDistricts(this.formChangeAddress.get('provinceCode')?.value).subscribe(res => {
        this.districts = res.response.data.districts;
      });
    }
  }
  wards: any;
  onDistrictChange() {
    if (this.formChangeAddress.get('districtCode')?.value) {
      this.locationService.getWards(this.formChangeAddress.get('districtCode')?.value).subscribe(res => {
        this.wards = res.response.data.wards;
      });
    }
  }

  changeAddress() {
    if (this.formChangeAddress.valid) {
      console.log(this.formChangeAddress.value)
      this.service.changeAddress(this.formChangeAddress.value).subscribe(res => {
        this.toastr.success('Thay đổi địa chỉ thành công!', 'Success');
      }, error => {
        this.toastr.error('Có lỗi vui lòng thử lại', 'Error');
      })
    } else {
      this.toastr.error('Nhập thông tin đầy đủ', 'Error');
    }
  }

}
