<script setup lang="ts">
import { FlashCardShort } from '@/models/FlashCardShort';
import { PageQuery } from '@/models/PageQuery';
import { ErrorHandler } from '@/services/ErrorHandler';
import { FlashCardService } from '@/services/FlashCardService';
import { onMounted, ref } from 'vue';

const service = new FlashCardService();
const total = ref(0);
const data = ref<FlashCardShort[]>([]);
const loading = ref(false);
const error = ref<string | null>(null);

const initView = async () => {
  await loadData(true);
}

const loadData = async (reload = false) => {
  try {
    loading.value = true;
    const skip = reload ? 0 : data.value.length;
    const query = new PageQuery(reload, skip);
    const page = await service.getByQuery(query);
    data.value = reload ? page.data : [...data.value, ...page.data];
    if (page.total != null) { // null or undefined
      total.value = page.total;
    }
  } catch (err) {
    data.value = [];
    total.value = 0;
    error.value = ErrorHandler.getMessage(err);
  } finally {
    loading.value = false;
  }
}

const onScrolled = async (ev: Event) => {
  const container = ev.target as HTMLDivElement;
  const threshold = 100;
  const toBottom = container.scrollHeight - (container.scrollTop + container.clientHeight);
  if (toBottom < threshold && !loading.value && data.value.length < total.value) {
    await loadData();
  }
};

onMounted(initView);

</script>

<template>
  <main class="flash-card-list-view">
    <div class="flash-card-list-view__header">
      <RouterLink to="/add" class="flash-card-list-view__add-word">Add</RouterLink>
      <RouterLink to="/session" class="flash-card-list-view__start-session">Learn</RouterLink>
      <div class="flash-card-list-view__words-count">Words: {{ total }}</div>
    </div>
    <div class="scroll-container" @scrollend="onScrolled">
      <div v-for="card of data" :key="card.id" class="flash-card-list-view__flash-card">
        <div class="flash-card-list-view__flash-card-title">
          {{ card.word }}
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped lang="scss">

.flash-card-list-view {

  display: flex;
  flex-direction: column;
  overflow: hidden;
  flex: 1;

  &__flash-card {
    height: 3rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border: 1px solid var(--color-border);
    margin-top: 2px;
  }

  &__flash-card-title {
    text-wrap: nowrap;
    font-size: 1.2rem;
  }

  &__header {
    height: 3rem;
    margin-top: 5px;
    margin-bottom: 5px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    border: 1px solid var(--color-border);
    padding: 0 1rem;
  }
}
</style>
