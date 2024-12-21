import { map, take } from 'rxjs';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';
import { DATA_BRAND_DT, DATA_BRAND_LT, DATA_CHIP, DATA_CHIP_APPLE, DATA_CHIP_LAPTOP, DATA_RAM, DATA_ROM, DATA_SCREEN } from '../product';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from '../product.service';
import { formatDate } from '@angular/common';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.scss']
})
export class ProductUpdateComponent {
  constructor(
    public dialogRef: MatDialogRef<ProductUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient, private formBuilder: FormBuilder, private toastr: ToastrService, private productService: ProductService) { }

  formUpdateproduct: any;


  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.formUpdateproduct = this.formBuilder.group({
      productId: this.data.productId,
      productName: this.formBuilder.control(this.data.productName, Validators.required),
      price: this.formBuilder.control(this.data.productPrice, Validators.compose([Validators.required])),
      priceSell: this.formBuilder.control(this.data.productSell),
      description: this.formBuilder.control(this.data.productDescription, Validators.required),
      productionDate: this.formBuilder.control(this.parseDate(this.data.productionDate), Validators.compose([Validators.required])),
      productQuantity: this.formBuilder.control(this.data.productQuantity, Validators.required),
      productStatus: this.formBuilder.control(this.data.productStatus, Validators.required),
      productCategoryId: this.formBuilder.control(this.data.categoryId, Validators.required),
      productBrand: this.formBuilder.control(this.data.productBrand, Validators.required),
      imageProducts: this.formBuilder.control(this.data.images),
      chipProduct: this.data.productConfig.chip,
      ramProduct: this.data.productConfig.ram,
      romProduct: this.data.productConfig.rom,
      screenProduct: this.data.productConfig.screen,
    });

    // Fetch and convert each imagePath
    const fetchImagePromises = this.data.images.map((image: any) => this.fetchImageAndConvert(image.imagePath));
    const imageFileApi = this.data.images.map((image: any) => image.imagePath);

    Promise.all(imageFileApi)
      .then(images => {
        // // Concatenate files with local images
        for (let index = 0; index < images.length; index++) {
          this.allImages.push(images[index])
        }
      })
      .catch(error => {
        console.error('Error show images:', error);
      });

    // Wait for all promises to resolve
    Promise.all(fetchImagePromises)
      .then(files => {
        // // Concatenate files with local images
        this.allFile = this.allFile.concat(files);
        this.formUpdateproduct.get('imageProducts')?.setValue(this.allFile);
      })
      .catch(error => {
        console.error('Error fetching and converting images:', error);
      });
  }

  // click send request
  processUpdate() {
    if (this.formUpdateproduct.valid && this.formUpdateproduct.get('price').value >= 0 && this.formUpdateproduct.get('productQuantity').value >= 0) {
      if (this.formUpdateproduct.get('priceSell').value < 0) {
        this.toastr.error('Giá khuyến mãi phải >= 0')
      }
      else if (this.allImages.length == 0) { this.toastr.error('Chọn ảnh cho sản phẩm') }
      else {
        if (!this.validateFileExtension(this.formUpdateproduct.get('imageProducts').value)) {
          console.log(this.formUpdateproduct.value)

          const formData = new FormData();

          Object.keys(this.formUpdateproduct.value).forEach(key => {
            if (this.formUpdateproduct.value[key] !== null && this.formUpdateproduct.value[key] !== undefined) {
              if (key === 'imageProducts') {
                for (let i = 0; i < this.formUpdateproduct.value[key].length; i++) {
                  formData.append(key, this.formUpdateproduct.value[key][i]);
                }
              }
              else if (key === 'productionDate') { formData.append(key, formatDate(this.formUpdateproduct.value[key], 'dd-MM-yyyy', 'en-US')); }
              else {
                formData.append(key, this.formUpdateproduct.value[key]);
              }
            }
          });
          this.productService.updateProduct(formData).subscribe(res => {
            console.log(res)
            this.toastr.success("Cập nhật thành công!")
            this.productService.fetchData();
            this.dialogRef.close();
          }, error => {
            console.log(error)
            this.toastr.error("Có lỗi xảy ra trong quá trình cập nhật!")
          })
        }
        else {
          this.toastr.error("file không đúng định dạng");
        }
      }
    }
    else {
      if (this.formUpdateproduct.get('productName').hasError('required')) {
        this.toastr.error("Yêu cầu nhập tên sản phẩm");
      }
      else if (this.formUpdateproduct.get('price').hasError('required')) {
        this.toastr.error("Nhập đúng giá sản phẩm");
      }
      else if (this.formUpdateproduct.get('description').hasError('required')) {
        this.toastr.error("Cần nhập mô tả sản phẩm");
      }
      else if (this.formUpdateproduct.get('productionDate').hasError('required')) {
        this.toastr.error("Nhập ngày sản xuất");
      }
      else if (this.formUpdateproduct.get('productQuantity').hasError('required')) {
        this.toastr.error("Cần nhập số lượng");
      }
      else if (this.formUpdateproduct.get('productStatus').hasError('required')) {
        this.toastr.error("Tình trạng sản phẩm");
      }
      else if (this.formUpdateproduct.get('productCategoryId').hasError('required')) {
        this.toastr.error("Chọn loại sản phẩm");
      }
      else if (this.formUpdateproduct.get('productBrand').hasError('required')) {
        this.toastr.error("Chọn hãng sản xuất");
      }
      else {
        this.toastr.error("Có trường nhập không đúng định dạng");
      }
    }
  }



  //#region config select img

  // Trong component TypeScript của bạn
  allImages: Array<string | ArrayBuffer | null> = [];
  allFile: Array<File | ArrayBuffer | null> = [];
  selectedImages: Array<string | ArrayBuffer | null> = []; // config before add

  onImagesSelected(event: any): void {
    const inputElement = event.target as HTMLInputElement;
    const files = inputElement.files;

    if (files) {
      // Lặp qua danh sách các tệp hình ảnh và đọc dữ liệu của chúng
      for (let i = 0; i < files.length; i++) {
        const reader = new FileReader();
        reader.onload = () => {
          this.allImages.push(reader.result);
        };
        reader.readAsDataURL(files[i]);
        this.allFile.push(files[i]);
      }
      this.formUpdateproduct.get('imageProducts')?.setValue(this.allFile);
    }
  }

  removeImage(index: number): void {
    // Loại bỏ tên file tại index 
    this.allFile.splice(index, 1);
    // Gán bản sao đã cập nhật lại cho imageProducts trên form
    this.formUpdateproduct.get('imageProducts')?.setValue(this.allFile);
    // Loại bỏ hình ảnh tại index khỏi mảng selectedImages
    this.allImages.splice(index, 1);
  }

  validateFileExtension(file: any) {
    const fileType: string[] = [];
    for (let i = 0; i < file.length; i++) {
      const fileT = file[i];
      fileType.push(fileT.name.split('.').pop());
    }
    return fileType.some(type => !['jpg', 'png'].includes(type));
  }
  //#endregion


  closePopUp() {
    this.dialogRef.close();
  }
  DATA_BRAND_DT = DATA_BRAND_DT

  // config for text editor
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    minHeight: '20rem',
    maxHeight: '20rem',
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

  // chip apple
  DATA_CHIP_APPLE = DATA_CHIP_APPLE
  // chip orther product
  DATA_CHIP = DATA_CHIP

  DATA_CHIP_LAPTOP = DATA_CHIP_LAPTOP

  DATA_BRAND_LT = DATA_BRAND_LT

  DATA_RAM = DATA_RAM
  // data rom
  rom = DATA_ROM;
  // screen
  dataScreen = DATA_SCREEN

  // convert imagePath to File
  fetchImageAndConvert(imagePath: string): Promise<File> {
    return new Promise((resolve, reject) => {
      fetch(imagePath, { mode: 'no-cors' })
        .then(response => response.blob())
        .then(blob => {
          const fileName = imagePath.substring(imagePath.lastIndexOf('/') + 1);
          const fileExtension = fileName.split('.').pop() || '';
          const file = new File([blob], fileName, { type: 'image/' + fileExtension });
          resolve(file);
        })
        .catch(error => reject(error));
    });
  }


  // Hàm chuyển đổi chuỗi thành đối tượng Date
  private parseDate(dateString: string): Date {
    const parts = dateString.split('-');
    const year = +parts[2];
    const month = +parts[1];
    const day = +parts[0];
    const dateP = month + '-' + day + '-' + year;
    console.log(new Date(dateP))
    return new Date(dateP);
  }

}
