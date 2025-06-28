<script setup lang="ts">
import type { CreateFlashCardRequest } from '@/models/CreateFlashCardRequest';
import { ErrorHandler } from '@/services/ErrorHandler';
import { FlashCardService } from '@/services/FlashCardService';
import { computed, ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const service = new FlashCardService();
const word = ref<string | null>(null);
const translation = ref<string | null>(null);
const loading = ref(false);
const error = ref<string | null>(null);

const isFormValid = computed(() => word.value && translation.value);

const resetForm = () => {
  word.value = null;
  translation.value = null;
}

const goToCardList = () => {
  resetForm();
  router.push('/');
}

const onAddClicked = async () => {
  if (!isFormValid) {
    return;
  }
  try {
    loading.value = true;
    const request : CreateFlashCardRequest = {
      word: word.value!,
      translation: translation.value!
    };
    await service.add(request);
    resetForm();
  } catch (err) {
    error.value = ErrorHandler.getMessage(err);
  } finally {
    loading.value = false;
  }
}

</script>

<template>
  <main class="add-flash-card-view">
    <div class="add-flash-card-view__form">
      <div class="add-flash-card-view__form-fields">
        <div class="add-flash-card-view__form-field">
          <span class="add-flash-card-view__form-field-label">Word: </span>
          <input class="add-flash-card-view__form-field-value" v-model="word">
        </div>
        <div class="add-flash-card-view__form-field">
          <span class="add-flash-card-view__form-field-label">Translation: </span>
          <input class="add-flash-card-view__form-field-value" v-model="translation">
        </div>
      </div>
      <div class="add-flash-card-view__form-buttons">
        <button class="add-flash-card-view__back-button secondary"
          @click="goToCardList()"
        >Back</button>
        <button class="add-flash-card-view__add-button primary"
          @click="onAddClicked()"
          :class="[isFormValid ? '' : 'disabled']"
        >Add</button>
      </div>
    </div>
  </main>
</template>

<style scoped lang="scss">

.add-flash-card-view {

  display: flex;
  flex-direction: column;
  overflow: hidden;
  flex: 1;

  &__form {
    margin: 10px 20px;
  }

  &__form-fields {
    display: flex;
    flex-direction: column;
    gap: 5px;
    margin-bottom: 20px;
  }

  &__form-field {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
  }

  &__form-buttons {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
    gap: 3px;
  }
}
</style>
