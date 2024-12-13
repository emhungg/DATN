import { Component } from '@angular/core';
import { NewsService } from '../news.service';

@Component({
  selector: 'app-list-all-news',
  templateUrl: './list-all-news.component.html',
  styleUrls: ['./list-all-news.component.css']
})
export class ListAllNewsComponent {


  listDataNews!: any[];
  constructor(private sv: NewsService) {

  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.sv.getListNews().subscribe(res => {
      this.listDataNews = res;
    })

  }

}


