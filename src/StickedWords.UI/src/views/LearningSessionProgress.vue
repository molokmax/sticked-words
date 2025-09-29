<script setup lang="ts">
import { LearningSession } from '@/models/LearningSession';
import type { LearningSessionStage } from '@/models/LearningSessionStage';

const props = defineProps({
  session: { type: LearningSession, required: true }
});

const getStageCompletePercentage = (stage: LearningSessionStage) => {
  const percentage = (stage.completedFlashCardCount / props.session.flashCardCount) * 100;

  return Math.ceil(percentage) + '%'
}

</script>

<template>
  <div class="learning-session-progress">
    <div v-for="stage of session.stages" :key="stage.id" class="learning-session-progress__stage">
      <div
        class="learning-session-progress__stage_completed"
        :style="{ width: getStageCompletePercentage(stage) }"
      ></div>
    </div>
  </div>
</template>

<style scoped lang="scss">

.learning-session-progress {
  display: flex;
  flex-direction: row;
  margin: 10px 0px;

  &__stage {
    flex: 1;
    margin: 2px;
    height: 5px;
    border: 1px solid var(--vt-c-text-green);
    border-radius: 3px;

    &_completed {
      height: 100%;
      background-color: var(--vt-c-text-green);
    }
  }
}
</style>
