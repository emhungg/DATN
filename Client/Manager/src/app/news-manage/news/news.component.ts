import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { NewsManageService } from '../news-manage.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss']
})
export class NewsComponent {
  productDialog: boolean = false;
  updateDialog: boolean = false;
  formCreateNew: any;
  dataSession: any;
  updateNews!: FormGroup;
  constructor(private service: NewsManageService, private formBuilder: FormBuilder, private toastr: ToastrService,) {
  }
  //#region  fillter
  textFillter: any;
  allObject: any;
  filterResults() {
    if (!this.textFillter) {
      this.POSTS = this.dataSession;
    }
    this.allObject = this.POSTS.filter((res: any) => {
      return res.title.toLowerCase().includes(this.textFillter.toLowerCase())
    }
    );
  }
  //#endregion
  //#region  sort data
  sortOrder: { key: string; reverse: boolean } = { key: '', reverse: false };

  sortObjects(key: string): void {
    console.log(key)
    this.sortOrder.reverse = !this.sortOrder.reverse;
    this.sortOrder.key = key;
    this.POSTS.sort((a: any, b: any) => {
      const valueA = a[key];
      const valueB = b[key];

      if (valueA < valueB) {
        return this.sortOrder.reverse ? 1 : -1;
      } else if (valueA > valueB) {
        return this.sortOrder.reverse ? -1 : 1;
      } else {
        return 0;
      }
    });
  }
  //#endregion
  //#region  get data
  private subscription!: Subscription;

  page: number = 1;
  count: number = 0;
  tableSize: number = 10;
  tableSizes: any = [10, 20, 50];
  POSTS: any;
  onTableDataChange(event: any) {
    this.page = event;

  }
  onTableSizeChange(event: any): void {
    this.tableSize = event.target.value;
    this.page = 1;

  }
  //#endregion

  ngOnInit(): void {
    this.subscription = this.service.data$.subscribe(data => {
      this.POSTS = data;
      this.dataSession = data;
    });

    // Khi component được khởi tạo, fetch dữ liệu lần đầu
    this.service.fetchData();
    this.formCreateNew = this.formBuilder.group({
      title: this.formBuilder.control('', Validators.required),
      image: this.formBuilder.control('', Validators.compose([Validators.required])),
      content: this.formBuilder.control('', Validators.required),
    });

    this.updateNews = this.formBuilder.group({
      title: this.formBuilder.control('', Validators.required),
      image: this.formBuilder.control('', Validators.compose([Validators.required])),
      content: this.formBuilder.control('', Validators.required),
    });
  }


  //#region  delete news
  processDelete(item: any) {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.processDeteleNew(item.idNews).subscribe(res => {
          console.log(res)
          if (!res) {
            Swal.fire({
              title: "Error!",
              text: "Có lỗi xảy ra trong quá trình thực hiện xóa.",
              icon: "error"
            });
          }
          Swal.fire({
            title: "Deleted!",
            text: "Đã xóa thành công.",
            icon: "success"
          });
          this.service.fetchData();
        }, error => {
          console.log(error)
          Swal.fire({
            title: "Error!",
            text: "Có lỗi xảy ra trong quá trình thực hiện xóa.",
            icon: "error"
          });
        });
      }
    });
  }
  //#endregion




  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    if (this.subscription) {
      this.subscription.unsubscribe();
      console.log('destroy');
    }

  }
  // config for text editor
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    minHeight: '30rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    sanitize: false,
    toolbarPosition: 'top',
    defaultFontName: 'Arial',
    customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText',
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
  };


  openFormCreate() {
    this.productDialog = true;
    this.removeImage()
  }

  idNews!: number;
  openUpdateForm(id: number) {
    this.removeImage();
    this.updateDialog = true;
    this.service.getNewsById(id).subscribe(
      (res) => {
        try {
          this.idNews = id;
          this.allFile = this.base64ToFile(res.image, 'image.png');

          if (this.allFile) {
            const reader = new FileReader();
            reader.onload = () => {
              this.selectedImage = reader.result;
            };
            reader.readAsDataURL(this.allFile);
            this.ImageUpdate?.setValue(this.allFile);
          }

          this.titleUpdate?.setValue(res.title);
          this.contentUpdate?.setValue(res.content);
        } catch (error) {
          console.error('Error processing image:', error);
        }
      },
      (error) => {
        console.error('Error fetching news by ID:', error);
      }
    );
  }

  closeDialog() {
    this.productDialog = false;
    this.updateDialog = false;
    this.updateNews.reset();
    this.formCreateNew.reset();
  }

  //#region config select img
  selectedImage!: string | ArrayBuffer | null; // Chỉ lưu một hình ảnh thay vì một mảng
  allFile!: File | ArrayBuffer | null; // Chỉ lưu một file thay vì một mảng

  onImagesSelected(event: any): void {
    const inputElement = event.target as HTMLInputElement;
    const files = inputElement.files;

    // Xóa tất cả các hình ảnh hiện có
    this.selectedImage = null;
    this.allFile = null;

    if (files && files.length > 0) {
      const reader = new FileReader();
      reader.onload = () => {
        this.selectedImage = reader.result;
      };
      reader.readAsDataURL(files[0]);
      this.allFile = files[0];
      if (this.productDialog) {
        this.formCreateNew.get('image')?.setValue(this.allFile);
      }
      if (this.updateDialog) {
        this.ImageUpdate?.setValue(this.allFile)
      }

    }
  }

  removeImage(): void {
    // Xóa hình ảnh hiện tại
    this.selectedImage = null;
    this.allFile = null;

    // Cập nhật giá trị trên form
    if (this.productDialog) {
      this.formCreateNew.get('image')?.setValue(this.allFile);
    }
    if (this.updateDialog) {
      this.ImageUpdate?.setValue(this.allFile)
    }
  }



  validateFileExtension(file: File): boolean {
    if (!file) {
      return false;
    }
    if (!(file instanceof File)) {
      return false;
    }
    const fileExtension = file.name.split('.').pop();
    const supportedExtensions = ['png', 'jpg'];
    if (!fileExtension) {
      return false;
    }
    return supportedExtensions.includes(fileExtension?.toLowerCase());
  }

  //#endregion


  processCreate() {
    if (this.formCreateNew.valid) {
      if (this.validateFileExtension(this.formCreateNew.get('image').value)) {
        const formData = new FormData();
        Object.keys(this.formCreateNew.value).forEach(key => {
          formData.append(key, this.formCreateNew.value[key])
        });
        this.service.createNews(formData).subscribe(res => {
          this.service.fetchData()
          this.toastr.success('Tạo thành công', 'Thông báo')
          this.closeDialog()
        }, error => {
          this.toastr.error("Có lỗi vui lòng thử lại!", "Error")
        })
      } else {
        this.toastr.error('file không dúng định đạng img/png', "Error")
      }
    } else {
      this.toastr.error('Vui lòng cung cấp đầy đủ thông tin', "Error")
    }
  }


  get ImageUpdate() {
    return this.updateNews.get('image');
  }
  get titleUpdate() {
    return this.updateNews.get('title');
  }
  get contentUpdate() {
    return this.updateNews.get('content');
  }

  base64ToFile(base64: string, filename: string): File | null {
    try {
      // Check if the base64 string is valid
      if (!base64 || typeof base64 !== 'string') {
        throw new Error('Invalid base64 string');
      }

      // Decode the base64 string
      const byteCharacters = atob(base64);
      const byteNumbers = new Array(byteCharacters.length);

      // Convert the decoded bytes to a Uint8Array
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);

      // Create a File object
      return new File([byteArray], filename, { type: 'image/png' });
    } catch (error) {
      console.error('Error decoding base64 string:', error);
      return null;
    }
  }


  processUpdateForm() {
    if (this.updateNews.valid) {
      if (this.validateFileExtension(this.ImageUpdate?.value)) {
        const formData = new FormData();
        Object.keys(this.updateNews.value).forEach(key => {
          formData.append(key, this.updateNews.value[key])
        });
        this.service.updateNews(formData, this.idNews).subscribe(res => {
          this.service.fetchData()
          this.toastr.success('Cập nhật thành công', 'Thông báo')
          this.closeDialog()
        }, error => {
          this.toastr.error("Có lỗi vui lòng thử lại!", "Error")
        })
      } else {
        this.toastr.error('file không dúng định đạng img/png', "Error")
      }
    } else {
      this.toastr.error('Vui lòng cung cấp đầy đủ thông tin', "Error")
    }
  }
}
