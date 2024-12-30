import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DemoButtonComponent } from './demo-button.component';

describe('DemoButtonComponent', () => {
  let component: DemoButtonComponent;
  let fixture: ComponentFixture<DemoButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DemoButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DemoButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
