import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdePageComponent } from './orde-page.component';

describe('OrdePageComponent', () => {
  let component: OrdePageComponent;
  let fixture: ComponentFixture<OrdePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OrdePageComponent]
    });
    fixture = TestBed.createComponent(OrdePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
