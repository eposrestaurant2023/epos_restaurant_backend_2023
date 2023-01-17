import { openDialog } from 'vue3-promise-dialog';
import ConfirmDialog from '../components/ComConfirm.vue';

export async function confirm(text) {
  return await openDialog(ConfirmDialog, text);
}
