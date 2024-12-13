import { Component } from '@angular/core';
import { HeaderService } from 'src/app/service/header.service';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-breadcrumd',
  templateUrl: './breadcrumd.component.html',
  styleUrls: ['./breadcrumd.component.scss']
})
export class BreadcrumdComponent {
/**
 *
 */
constructor(public bcService:BreadcrumbService, public headerService : HeaderService) {
  
}
}
