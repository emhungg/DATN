import { Component } from '@angular/core';
import { HeaderService } from 'src/app/service/header.service';
import { CoreService } from '../core.service';
import Swal from 'sweetalert2';

@Component({
  selector: "app-footer",
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
  email!: string;
  constructor(public footerHidden: HeaderService, private service: CoreService) {

  }

  async sendMailAdsToUserRegister() {
    await this.service.sendMailUserRegisterAds(this.email).subscribe(res => {
      Swal.fire("Cảm ơn bạn đã đăng ký dịch vụ của chúng tôi")
      this.email = ''
    }, error => {
      console.log(error)
      Swal.fire("Email không chính xác.")
      this.email = ''
    });
  }

  isHiddenSocial = true;
  hidenSocialMedia() {
    this.isHiddenSocial = !this.isHiddenSocial;
  }

}
