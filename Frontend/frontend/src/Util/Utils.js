export default class Utils {
  static dateFormatter(dateToFormat) {
    const date = new Date(dateToFormat);
    return `${date.getDay()}/${date.getMonth()}/${date.getFullYear()}`;
  }
}
