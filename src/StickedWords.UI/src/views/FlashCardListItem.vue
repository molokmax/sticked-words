<script setup lang="ts">
import { FlashCardShort } from '@/models/FlashCardShort';
import { onMounted, ref } from 'vue';

const props = defineProps({
  flashCard: { type: FlashCardShort, required: true }
});


const rateLevel = ref<number>(0);
const rateCssClass = ref<string | undefined>();

const initView = () => {
  const rate = getRate(props.flashCard);
  rateLevel.value = getRateLevel(rate);
  // rateCssClass.value = ``
}

const getRateLevel = (rate: number) => {
  if (rate == 100) {
    return 3;
  }

  if (rate > 50) {
    return 2;
  }

  return 1;
}

const getRate = (flashCard: FlashCardShort) => {
  if (flashCard.repeatAt > new Date()) {
    return 100;
  }

  return flashCard.rate;
}

onMounted(initView);

</script>

<template>
  <div class="flash-card-list-item">
    <div class="flash-card-list-item__title">
      {{ flashCard.word }}
    </div>
    <div class="flash-card-list-item__space"></div>
    <div class="flash-card-list-item__rate">
      <img v-if="rateLevel === 1" src="/low-rate.png" alt="low-rate">
      <img v-if="rateLevel === 2" src="/medium-rate.png" alt="medium-rate">
      <img v-if="rateLevel === 3" src="/high-rate.png" alt="high-rate">
    </div>
  </div>
</template>

<style scoped lang="scss">

.flash-card-list-item {

  height: 3rem;
  padding: 0px 30px;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  border: 1px solid var(--color-border);

  &__space {
    flex: 1;
  }

  &__title {
    text-wrap: nowrap;
    font-size: 1.2rem;
  }

  &__rate {
    display: flex;

    img {
      height: 1.5rem;
      width: 1.5rem;
    }
  }
}
</style>
