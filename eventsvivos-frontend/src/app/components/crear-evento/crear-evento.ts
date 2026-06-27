import { Component, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { EventoService } from '../../services/evento';

@Component({
  selector: 'app-crear-evento',
  imports: [FormsModule, RouterLink],
  templateUrl: './crear-evento.html',
  styleUrl: './crear-evento.scss'
})
export class CrearEvento {
  evento = {
    titulo: '',
    descripcion: '',
    venueId: 1,
    capacidadMaxima: 0,
    fechaHoraInicio: '',
    fechaHoraFin: '',
    precioEntrada: 0,
    tipoEvento: 'conferencia'
  };

  mensaje = '';
  error = '';

  constructor(
    private eventoService: EventoService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  crear() {
    this.mensaje = '';
    this.error = '';
    this.eventoService.crearEvento(this.evento).subscribe({
      next: () => {
        this.mensaje = 'Evento creado exitosamente';
        this.cdr.detectChanges();
        setTimeout(() => this.router.navigate(['/eventos']), 1500);
      },
      error: (err) => {
        if (err.error?.errors) {
          const errores = Object.values(err.error.errors).flat();
          this.error = (errores as string[]).join(', ');
        } else if (err.error?.error) {
          this.error = err.error.error;
        } else {
          this.error = 'Error al crear el evento';
        }
        this.cdr.detectChanges();
      }
    });
  }
}