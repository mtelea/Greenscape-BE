import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FructeComponent } from './fructe.component';

describe('FructeComponent', () => {
  let component: FructeComponent;
  let fixture: ComponentFixture<FructeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FructeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FructeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
