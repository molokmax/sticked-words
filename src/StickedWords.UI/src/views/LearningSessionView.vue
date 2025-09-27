<script setup lang="ts">
import { ExerciseType, LearningSessionState } from '@/models/LearningSession';
import { ErrorHandler } from '@/services/ErrorHandler';
import { LearningSessionService } from '@/services/LearningSessionService';
import { computed, onMounted, ref } from 'vue';
import TranslateForeignToNativeExerciseView from './exercises/TranslateForeignToNativeExerciseView.vue';
import TranslateNativeToForeignExerciseView from './exercises/TranslateNativeToForeignExerciseView.vue';
import LearningSessionResultsView from './LearningSessionResultsView.vue';

const service = new LearningSessionService();
const cardCount = ref<number>(0);
const sessionState = ref<LearningSessionState>(LearningSessionState.None);
const exerciseType = ref<ExerciseType>(ExerciseType.None);
const sessionId = ref<number>(-1);
const flashCardId = ref<number | undefined>(undefined);
const loading = ref(false);
const error = ref<string | null>(null);

const isSessionCompleted = computed(() => sessionState.value === LearningSessionState.Finished || sessionState.value === LearningSessionState.Expired);

const initView = async () => {
  await loadData();
}

const loadLearningSession = async () => {
  const sessionId = service.getCurrentSessionId();
  if (sessionId) {
    const session = await service.getById(sessionId);
    if (session) {
      return session;
    }
  }

  const session = await service.getActive() ?? await service.start();
  service.saveCurrentSessionId(session.id);

  return session;
}

const loadData = async () => {
  try {
    loading.value = true;

    const session = await loadLearningSession();
    sessionId.value = session.id;
    sessionState.value = session.state;
    flashCardId.value = session.flashCardId;
    exerciseType.value = session.exerciseType;
    cardCount.value = session.flashCardCount;
  } catch (err) {
    sessionId.value = -1;
    sessionState.value = LearningSessionState.None;
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
    <div v-if="isSessionCompleted">
      <LearningSessionResultsView
        :sessionId="sessionId"
      ></LearningSessionResultsView>
    </div>
    <div v-else-if="flashCardId">
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
