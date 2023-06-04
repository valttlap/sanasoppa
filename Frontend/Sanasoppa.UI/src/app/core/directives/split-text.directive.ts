import { AfterViewInit, Directive, ElementRef } from '@angular/core';

@Directive({
  selector: '[appSplitText]',
})
export class SplitTextDirective implements AfterViewInit {
  constructor(private el: ElementRef) {}

  ngAfterViewInit(): void {
    const text = this.el.nativeElement.innerText;
    this.el.nativeElement.innerHTML = '';
    Array.from(text).forEach((letter, index) => {
      this.el.nativeElement.innerHTML += `<span style="animation-delay: ${
        index * 75
      }ms">${letter}</span>`;
    });
  }
}
