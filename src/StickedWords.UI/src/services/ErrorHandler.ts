export class ErrorHandler {

  static getMessage(error: any): string | null {
    if (error instanceof Error) {
      return error.message;
    }

    return 'Unknown error';
  }
}
