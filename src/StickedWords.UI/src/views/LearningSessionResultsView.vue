<script setup lang="ts">
import { LearningSessionState } from '@/models/LearningSession';
import { ErrorHandler } from '@/services/ErrorHandler';
import { LearningSessionService } from '@/services/LearningSessionService';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

const props = defineProps({
  sessionId: { type: Number, required: true }
});

const router = useRouter();
const service = new LearningSessionService();
const sessionState = ref<LearningSessionState>(LearningSessionState.None);
const cardCount = ref<number>(0);
const loading = ref(false);
const error = ref<string | null>(null);

const initView = async () => {
  await loadData();
}

const loadData = async () => {
  try {
    loading.value = true;

    const results = await service.getResults(props.sessionId);
    sessionState.value = results.state;
    cardCount.value = results.flashCardCount;
  } catch (err) {
    sessionState.value = LearningSessionState.None;
    cardCount.value = 0;
    error.value = ErrorHandler.getMessage(err);
  } finally {
    loading.value = false;
  }
}

const onCompleteClicked = () => {
  service.deleteCurrentSessionId();
  router.push('/');
}

onMounted(initView);

</script>

<template>
  <main class="learning-session-results-view">
    <div
      v-if="sessionState === LearningSessionState.Expired"
      class="learning-session-results-view__title"
    >
      Session expired
    </div>
    <div
      v-if="sessionState === LearningSessionState.Finished"
      class="learning-session-results-view__title"
    >
      Session finished
    </div>
    <div class="learning-session-results-view__description">
      Learned words: {{ cardCount }}
    </div>
    <div class="learning-session-results-view__buttons">
      <button class="learning-session-results-view__add-button primary"
        @click="onCompleteClicked()"
        :class="[loading ? 'disabled' : '']"
      >Complete</button>
    </div>
  </main>
</template>

<style scoped lang="scss">

.learning-session-results-view {
  display: flex;
  flex-direction: column;
  overflow: hidden;
  flex: 1;

  &__title {
    font-weight: 600;
  }

  &__description {
    margin-top: 10px;
    margin-bottom: 30px;
  }

  &__buttons {
    margin-top: 20px;
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
  }
}
</style>
