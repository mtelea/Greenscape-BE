import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FloriDetailsComponent } from './flori-details.component';

describe('FloriDetailsComponent', () => {
  let component: FloriDetailsComponent;
  let fixture: ComponentFixture<FloriDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FloriDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FloriDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
