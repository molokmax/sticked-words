<script setup lang="ts">
import type { TranslateGuess } from '@/models/exercises/TranslateGuess';
import { GuessResult, TranslateGuessResult } from '@/models/exercises/TranslateGuessResult';
import { ErrorHandler } from '@/services/ErrorHandler';
import { TranslateNativeToForeignExerciseService } from '@/services/exercises/TranslateNativeToForeignExerciseService';
import { computed, onMounted, ref, watch } from 'vue';

const props = defineProps({
  flashCardId: { type: Number, required: true }
});

const emit = defineEmits(['next']);

const service = new TranslateNativeToForeignExerciseService();
const word = ref<string>("");
const answer = ref<string>("");
const correctAnswer = ref<string | null>(null);
const loading = ref(false);
const isGuessChecked = ref(false);
const isGuessCorrect = ref(false);
const error = ref<string | null>(null);

const isFormValid = computed(() => answer.value.trim().length > 0);

const initView = async () => {
  await loadData(props.flashCardId);
}

watch(() => props.flashCardId, async (newFlashCardId, _) => {
  await loadData(newFlashCardId);
})

const resetForm = () => {
  answer.value = '';
  isGuessChecked.value = false;
  isGuessCorrect.value = false;
  correctAnswer.value = null;
}

const setCheckResult = (result: TranslateGuessResult) => {
  isGuessChecked.value = true;
  isGuessCorrect.value = result.result === GuessResult.Correct;
  correctAnswer.value = result.correctTranslation ?? null;
}

const loadData = async (flashCardId: number) => {
  try {
    loading.value = true;

    resetForm();
    const exercise = await service.get(flashCardId);
    word.value = exercise.word;
  } catch (err) {
    word.value = "";
    error.value = ErrorHandler.getMessage(err);
  } finally {
    loading.value = false;
  }
}

const onCheckClicked = async () => {
  if (!isFormValid) {
    return;
  }
  try {
    loading.value = true;
    const request : TranslateGuess = {
      flashCardId: props.flashCardId,
      answer: answer.value
    };
    const response = await service.check(request);
    if (response.result === GuessResult.None) {
      throw new Error(`Guess result '${response.result}' is not supported`);
    }
    setCheckResult(response);
  } catch (err) {
    error.value = ErrorHandler.getMessage(err);
  } finally {
    loading.value = false;
  }
}

const onNextClicked = () => emit('next');

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
    <input
      class="translate-native-to-foreign-exercise-view__answer"
      :disabled="isGuessChecked"
      v-model="answer"
    >
    <div class="translate-native-to-foreign-exercise-view__results">
      <div
        v-if="isGuessChecked && isGuessCorrect"
        class="translate-native-to-foreign-exercise-view__correct-result"
      >
        <div class="translate-native-to-foreign-exercise-view__result-label">&check; Right!</div>
      </div>
      <div
        v-else-if="isGuessChecked && !isGuessCorrect"
        class="translate-native-to-foreign-exercise-view__wrong-result"
      >
        <div class="translate-native-to-foreign-exercise-view__result-label">&cross; Wrong</div>
        <div class="translate-native-to-foreign-exercise-view__result-label translate-native-to-foreign-exercise-view__correct-answer">
          {{ correctAnswer }}
        </div>
      </div>
    </div>
    <div class="translate-native-to-foreign-exercise-view__buttons">
      <button
        v-if="isGuessChecked"
        class="translate-native-to-foreign-exercise-view__check-button primary"
        @click="onNextClicked()"
      >
        Next
      </button>
      <button
        v-else
        class="translate-native-to-foreign-exercise-view__check-button primary"
        :disabled="!isFormValid"
        @click="onCheckClicked()"
      >
        Check
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

  &__results {
    height: 60px;
  }

  &__correct-result,
  &__wrong-result {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding-top: 10px;
    font-size: 0.8em;
  }

  &__correct-result &__result-label {
    color: var(--color-text-good);
  }

  &__wrong-result &__result-label {
    color: var(--color-text-bad);
  }

  &__wrong-result &__correct-answer {
    font-size: 1.5em;
    font-style: italic;
    color: var(--color-text-good);
  }

  &__buttons {
    margin-top: 20px;
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
  }
}
</style>
