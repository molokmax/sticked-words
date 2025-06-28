<script setup lang="ts">
import { ErrorHandler } from '@/services/ErrorHandler';
import { TranslateNativeToForeignExerciseService } from '@/services/exercises/TranslateNativeToForeignExerciseService';
import { onMounted, ref } from 'vue';

// TODO: реализовать получение результата
// TODO: реализовать отображение результата

const props = defineProps({
  flashCardId: { type: Number, required: true }
});

const service = new TranslateNativeToForeignExerciseService();
const word = ref<string>("");
const answer = ref<string>("");
const loading = ref(false);
const error = ref<string | null>(null);

const initView = async () => {
  await loadData();
}

const loadData = async () => {
  try {
    loading.value = true;

    const exercise = await service.get(props.flashCardId);
    word.value = exercise.word;
  } catch (err) {
    word.value = "";
    error.value = ErrorHandler.getMessage(err);
  } finally {
    loading.value = false;
  }
}

onMounted(initView);

</script>

<template>
  <main class="translate-native-to-foreign-exercise-view">
    <div class="translate-native-to-foreign-exercise-view__exercise-description">
      Translate to foreign:
    </div>
    <div class="translate-native-to-foreign-exercise-view__word">
      {{ word }}
    </div>
    <input class="translate-native-to-foreign-exercise-view__answer" v-model="answer">
    <div class="translate-native-to-foreign-exercise-view__buttons">
      <button class="translate-native-to-foreign-exercise-view__send-button primary">
        Send
      </button>
    </div>
  </main>
</template>

<style scoped lang="scss">

.translate-native-to-foreign-exercise-view {

  display: flex;
  flex-direction: column;
  overflow: hidden;

  width: 300px;

  &__exercise-description {
    margin: 10px;
    font-style: italic;
  }

  &__word {
    font-weight: 600;
    margin: 25px auto;
  }

  &__answer {
    width: 100%;
  }

  &__buttons {
    margin-top: 20px;
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
  }
}
</style>
