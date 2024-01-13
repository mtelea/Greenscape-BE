import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FructeDetailsComponent } from './fructe-details.component';

describe('FructeDetailsComponent', () => {
  let component: FructeDetailsComponent;
  let fixture: ComponentFixture<FructeDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FructeDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FructeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
