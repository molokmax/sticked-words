<script setup lang="ts">
import { ExerciseType } from '@/models/LearningSession';
import { ErrorHandler } from '@/services/ErrorHandler';
import { LearningSessionService } from '@/services/LearningSessionService';
import { onMounted, ref } from 'vue';
import TranslateForeignToNativeExerciseView from './exercises/TranslateForeignToNativeExerciseView.vue';
import TranslateNativeToForeignExerciseView from './exercises/TranslateNativeToForeignExerciseView.vue';

const service = new LearningSessionService();
const cardCount = ref<number>(0);
const exerciseType = ref<ExerciseType>(ExerciseType.None);
const flashCardId = ref<number | undefined>(undefined);
const loading = ref(false);
const error = ref<string | null>(null);

const initView = async () => {
  await loadData();
}

const loadData = async () => {
  try {
    loading.value = true;

    let session = await service.getActive();
    if (!session) {
      session = await service.start();
    }

    flashCardId.value = session.flashCardId;
    exerciseType.value = session.exerciseType;
    cardCount.value = session.flashCardCount;
  } catch (err) {
    cardCount.value = 0;
    exerciseType.value = ExerciseType.None;
    flashCardId.value = undefined;
    error.value = ErrorHandler.getMessage(err);
  } finally {
    loading.value = false;
  }
}

onMounted(initView);

</script>

<template>
  <main class="learning-session-view">
    <div v-if="flashCardId">
      <div v-if="exerciseType === ExerciseType.TranslateForeignToNative">
        <TranslateForeignToNativeExerciseView
          :flashCardId="flashCardId"
          @next="loadData()"
        ></TranslateForeignToNativeExerciseView>
      </div>
      <div v-else-if="exerciseType === ExerciseType.TranslateNativeToForeign">
        <TranslateNativeToForeignExerciseView
          :flashCardId="flashCardId"
          @next="loadData()"
        ></TranslateNativeToForeignExerciseView>
      </div>
      <div v-else>
        Unknown Exercise
      </div>
    </div>
  </main>
</template>

<style scoped lang="scss">

.learning-session-view {
  display: flex;
  flex-direction: column;
  overflow: hidden;
  flex: 1;

  border: 1px solid var(--color-border);
  margin-top: 2px;
  padding: 30px;
  align-items: center;
}
</style>
