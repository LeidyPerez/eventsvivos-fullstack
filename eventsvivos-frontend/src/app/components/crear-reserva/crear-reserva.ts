import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { EventoService } from '../../services/evento';

@Component({
  selector: 'app-crear-reserva',
  imports: [FormsModule, RouterLink],
  templateUrl: './crear-reserva.html',
  styleUrl: './crear-reserva.scss'
})
export class CrearReserva implements OnInit {
  eventoId = '';
  reserva = {
    eventoId: '',
    cantidad: 1,
    nombreComprador: '',
    emailComprador: ''
  };

  mensaje = '';
  error = '';
  reservaCreada: any = null;

  constructor(
    private eventoService: EventoService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.eventoId = this.route.snapshot.paramMap.get('id') || '';
    this.reserva.eventoId = this.eventoId;
  }

  crear() {
    this.mensaje = '';
    this.error = '';
    this.eventoService.crearReserva(this.reserva).subscribe({
      next: (data) => {
        this.reservaCreada = data;
        this.mensaje = 'Reserva creada exitosamente';
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = err.error?.error || 'Error al crear la reserva';
        this.cdr.detectChanges();
      }
    });
  }

  confirmarPago() {
    this.eventoService.confirmarPago(this.reservaCreada.id).subscribe({
      next: (data) => {
        this.reservaCreada = data;
        this.mensaje = `Pago confirmado. Código: ${data.codigoReserva}`;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = err.error?.error || 'Error al confirmar el pago';
        this.cdr.detectChanges();
      }
    });
  }

  cancelar() {
    this.eventoService.cancelarReserva(this.reservaCreada.id).subscribe({
      next: () => {
        this.mensaje = 'Reserva cancelada';
        this.reservaCreada = null;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = err.error?.error || 'Error al cancelar';
        this.cdr.detectChanges();
      }
    });
  }
}