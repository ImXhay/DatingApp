import { Component, input, output, signal } from '@angular/core';

@Component({
  selector: 'app-image-upload',
  standalone: true, // Assuming modern Angular based on your imports
  imports: [],
  templateUrl: './image-upload.html',
  styleUrl: './image-upload.css',
})
export class ImageUpload {
  protected imgeSrc = signal<string | ArrayBuffer | null | undefined>(null);
  protected isDraggeing = false;
  private fileToUpload: File | null = null;

  uploadFile = output<File>();
  loading = input<boolean>(false);

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.fileToUpload = file;
      this.previewImage(file);
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDraggeing = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDraggeing = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDraggeing = false;

    if (event.dataTransfer?.files.length) {
      const file = event.dataTransfer.files[0];
      this.fileToUpload = file;
      this.previewImage(file);
    }
  }

  onCancel() {
    this.fileToUpload = null;
    this.imgeSrc.set(null);
  }

  onUploadFile() {
    if (this.fileToUpload) {
      this.uploadFile.emit(this.fileToUpload);
    }
  }

  private previewImage(file: File) {
    const reader = new FileReader();
    reader.onload = (e) => this.imgeSrc.set(e.target?.result);
    reader.readAsDataURL(file);
  }
}