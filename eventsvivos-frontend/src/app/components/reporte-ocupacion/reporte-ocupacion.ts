import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { EventoService } from '../../services/evento';

@Component({
  selector: 'app-reporte-ocupacion',
  imports: [CommonModule, RouterLink],
  templateUrl: './reporte-ocupacion.html',
  styleUrl: './reporte-ocupacion.scss'
})
export class ReporteOcupacion implements OnInit {
  reporte: any = null;
  error = '';

  constructor(
    private eventoService: EventoService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id') || '';
    this.eventoService.obtenerReporte(id).subscribe({
      next: (data) => {
        this.reporte = data;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = err.error?.error || 'Error al obtener el reporte';
        this.cdr.detectChanges();
      }
    });
  }
}