import { formatDate } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from '../product.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { DATA_BRAND_DT, DATA_BRAND_LT, DATA_CHIP, DATA_CHIP_APPLE, DATA_CHIP_LAPTOP, DATA_RAM, DATA_ROM, DATA_SCREEN } from '../product';

@Component({
  selector: 'app-product-add',
  templateUrl: './product-add.component.html',
  styleUrls: ['./product-add.component.scss']
})
export class ProductAddComponent {
  /**
   *
   */
  formCreateNew: any;
  constructor(public dialogRef: MatDialogRef<ProductAddComponent>, private formBuilder: FormBuilder, private toastr: ToastrService, private productService: ProductService) {

  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.formCreateNew = this.formBuilder.group({
      productName: this.formBuilder.control('', Validators.required),
      price: this.formBuilder.control('0', Validators.compose([Validators.required])),
      priceSell: this.formBuilder.control('0'),
      description: this.formBuilder.control('', Validators.required),
      productionDate: this.formBuilder.control(new Date(), Validators.compose([Validators.required])),
      productQuantity: this.formBuilder.control('0', Validators.required),
      productStatus: this.formBuilder.control('', Validators.required),
      productCategoryId: this.formBuilder.control('', Validators.required),
      productBrand: this.formBuilder.control('', Validators.required),
      imageProducts: this.formBuilder.control(['']),
      chipProduct: this.formBuilder.control(null),
      ramProduct: this.formBuilder.control(null),
      romProduct: this.formBuilder.control(null),
      screenProduct: this.formBuilder.control(null),
    });
  }

  actualDateFormGroup = new Date(new Date().getTime() - (new Date().getTimezoneOffset() * 60000)).toISOString().split("T")[0];

  onMyDateChange(event: any) {
    this.formCreateNew.patchValue({ myDate: event.target.value });
  }

  // click send request
  processCreate() {
    if (this.formCreateNew.valid && this.formCreateNew.get('price').value >= 0 && this.formCreateNew.get('productQuantity').value >= 0) {
      if (this.formCreateNew.get('priceSell').value < 0) {
        this.toastr.error('Giá khuyến mãi phải >= 0')
      }
      else if (this.selectedImages.length == 0) { this.toastr.error('Chọn ảnh cho sản phẩm') }
      else {
        if (!this.validateFileExtension(this.formCreateNew.get('imageProducts').value)) {
          console.log(this.formCreateNew.value)
          const formData = new FormData();
          Object.keys(this.formCreateNew.value).forEach(key => {
            if (this.formCreateNew.value[key] !== null && this.formCreateNew.value[key] !== undefined) {
              if (key === 'imageProducts') {
                for (let i = 0; i < this.formCreateNew.value[key].length; i++) {
                  formData.append(key, this.formCreateNew.value[key][i]);
                }
              }
              else if (key === 'productionDate') { formData.append(key, formatDate(this.formCreateNew.value[key], 'dd-MM-yyyy', 'en-US')); }
              else {
                formData.append(key, this.formCreateNew.value[key]);
              }
            }
          });
          this.productService.createProduct(formData).subscribe(res => {
            console.log(res)
            this.toastr.success("Thêm thành công sản phẩm mới!")
            this.productService.fetchData();
          }, error => {
            console.log(error)
            this.toastr.error("Có lỗi xảy ra trong quá trình tạo mới!")
          })
        }
        else {
          this.toastr.error("file không đúng định dạng");
        }
      }
    }
    else {
      if (this.formCreateNew.get('productName').hasError('required')) {
        this.toastr.error("Yêu cầu nhập tên sản phẩm");
      }
      else if (this.formCreateNew.get('price').hasError('required')) {
        this.toastr.error("Nhập đúng giá sản phẩm");
      }
      else if (this.formCreateNew.get('description').hasError('required')) {
        this.toastr.error("Cần nhập mô tả sản phẩm");
      }
      else if (this.formCreateNew.get('productionDate').hasError('required')) {
        this.toastr.error("Nhập ngày sản xuất");
      }
      else if (this.formCreateNew.get('productQuantity').hasError('required')) {
        this.toastr.error("Cần nhập số lượng");
      }
      else if (this.formCreateNew.get('productStatus').hasError('required')) {
        this.toastr.error("Tình trạng sản phẩm");
      }
      else if (this.formCreateNew.get('productCategoryId').hasError('required')) {
        this.toastr.error("Chọn loại sản phẩm");
      }
      else if (this.formCreateNew.get('productBrand').hasError('required')) {
        this.toastr.error("Chọn hãng sản xuất");
      }
      else {
        this.toastr.error("Có trường nhập không đúng định dạng");
      }
    }
  }


  //#region config select img
  selectedImages: Array<string | ArrayBuffer | null> = [];
  allFile: Array<File | ArrayBuffer | null> = [];
  onImagesSelected(event: any): void {
    const inputElement = event.target as HTMLInputElement;
    const files = inputElement.files;

    if (files) {
      // Lặp qua danh sách các tệp hình ảnh và đọc dữ liệu của chúng
      for (let i = 0; i < files.length; i++) {
        const reader = new FileReader();
        reader.onload = () => {
          this.selectedImages.push(reader.result);
        };
        reader.readAsDataURL(files[i]);
        this.allFile.push(files[i]);
      }

      this.formCreateNew.get('imageProducts')?.setValue(this.allFile);
    }
  }

  removeImage(index: number): void {
    // Loại bỏ tên file tại index
    this.allFile.splice(index, 1);

    // Gán bản sao đã cập nhật lại cho imageProducts trên form
    this.formCreateNew.get('imageProducts')?.setValue(this.allFile);

    // Loại bỏ hình ảnh tại index khỏi mảng selectedImages
    this.selectedImages.splice(index, 1);
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
}

