# StudySystem

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 16.2.0.

## Toastr Notifications

npm install --save ngx-toastr then <br/>
"styles": [<br/>
"@angular/material/prebuilt-themes/indigo-pink.css",<br/>
"node_modules/bootstrap/dist/css/bootstrap.min.css", // bootstrap<br/>
"node_modules/ngx-toastr/toastr.css", // toastr<br/>
"src/styles.css"<br/>
],<br/>
finaly: import to app.module.ts ToastrModule.forRoot()<br/>

## Material

create file material.module.ts and add library to exports

## Environments

ng g environments<br/>
add base environment to environemnt.development.ts and environemnt.ts

## Bootstrap

npm install bootstrap, as follows:<br/>
"styles": [<br/>
"@angular/material/prebuilt-themes/indigo-pink.css",<br/>
"node_modules/bootstrap/dist/css/bootstrap.min.css", // bootstrap<br/>
"node_modules/ngx-toastr/toastr.css", // toastr<br/>
"src/styles.css"<br/>
],<br/>
"scripts": [<br/>
"node_modules/bootstrap/dist/js/bootstrap.min.js", // bootstrap<br/>
"node_modules/jquery/dist/jquery.min.js"<br/>
]<br/>

## fxFlex API in Angular flex layout

npm i -s @angular/flex-layout @angular/cdk<br/>

<b>app.module.ts</b><br/>

import { FlexLayoutModule } from '@angular/flex-layout';<br/>
...<br/>

@NgModule({<br/>
    ...<br/>
    imports: [ FlexLayoutModule ],<br/>
    ...<br/>
});<br/>

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.
