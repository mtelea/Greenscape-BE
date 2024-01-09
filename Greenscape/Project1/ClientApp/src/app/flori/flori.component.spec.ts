import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FloriComponent } from './flori.component';

describe('FloriComponent', () => {
  let component: FloriComponent;
  let fixture: ComponentFixture<FloriComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FloriComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FloriComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
