import random
import pandas as pd
import pickle
import numpy as np
import sys
np.set_printoptions(threshold=sys.maxsize)

#################################################################
#                      WRITING OUTPUT                           #
#################################################################

def formatDataframe(dataframe, output, skip = False):
  df = dataframe.astype(str)

  if (skip == False):
    df.to_csv(output + ".csv", index=False)

  df_arr = df.to_numpy()
  df_str = str(df_arr).replace("\n", ",").strip()
  df_elem = str(df_str).replace("' '", "', '").strip()
  df_elem = str(df_elem).replace("[", "{").strip()
  df_elem = str(df_elem).replace("]", "}").strip()
  df_elem = str(df_elem).replace("'", "\"").strip()

  with open(output + ".txt", "w") as output:
    output.write(df_elem)

#################################################################
#                    PATH VIDEO TASK                            #
#################################################################

# generate path video tasks
def generate_path(
  options = 6,        # number of videos available
  prompts = 50,       # number of questions to generate
  prompt_length = 5   # number of videos to show per question
):
  questions = []      # video sequences   --> column 1
  answer_orders = []  # answer sequences  --> column 2

  while len(questions) < prompts:
    # get unique random numbers for video order
    values = random.sample(range(1, options), prompt_length)
    for i in range(0, len(values)):
      values[i] = str(values[i])
    
    # check if question already exists
    question = ''.join(values)
    if question not in questions:
      print("adding " + question + " with " + str(prompts - len(questions) - 1) + " prompts left")
      
      # generate answer order by shuffing video order
      random.shuffle(values)
      answer_order = ''.join(values)
      
      # save question and answer orders
      questions.append(question)
      answer_orders.append(answer_order)

  # export questions with answer sequence to csv
  df = pd.DataFrame({'question': questions, 'answer_order': answer_orders, 'interrupts':np.ones(prompts)})
  formatDataframe(df, "path-questions")
  # df.to_csv("path-questions.csv", index=False)

def readPathStructure():
  df = pd.read_csv('path-structure.csv')
  formatDataframe(df, "path-structure", skip = True)

#################################################################
#                     STROOP COLOR TASK                         #
#################################################################

# sample elements from a given population
def sample(population, k):
  sample = []
  for i in range(0, k):
    sample.append(random.choice(population))
  return sample

# get all but a particular item from the population
def allbut(population, item):
  allbut = []
  for i in range(0, len(population)):
    if (population[i] != item):
      allbut.append(population[i])
  return allbut

# generate stroop color tasks  
def generate_stroop(
  options,      # array of color options
  prompts,      # number of questions to generate
  ratio = 0.5   # ratio of yes / no answers
):
  text_1 = sample(options, prompts)     # text left           --> column 1
  text_2 = sample(options, prompts)     # text right          --> column 3
  color_1 = sample(options, prompts)    # color of text left  --> column 2
  color_2 = []                          # color of text right --> column 4
  answers = []                          # correct answers     --> column 5

  # generate answers
  for i in range(0, prompts):
    weighted_coin = random.uniform(0, 1)
    if weighted_coin <= ratio:
      color_2.append(text_1[i])
      answers.append("1")
    else:
      alternatives = allbut(options, text_1[i])
      color_2.append(random.choice(alternatives))
      answers.append("0")

  # export prompts with answers to csv
  df = pd.DataFrame({'text_1': text_1, 'color_1': color_1, 'text_2': text_2, 'color_2': color_2, 'answer': answers})
  formatDataframe(df, "stroop-questions")

#################################################################
#                     MATH AREA TASK                            #
#################################################################

# determine solution of a generated math problem
def get_solution(operand_1, operand_2, operation):
  solution = -1
  if (operation == "+"): # add
    solution = operand_1 + operand_2
  elif (operation == "-"): # subtract
    solution = operand_1 - operand_2
  elif (operation == "*"): # multiply
    solution = operand_1 * operand_2
  # elif (operation == "/"): # divide
  #  solution = operand_1 / float(operand_2)
  return solution

# generate math area tasks
def generate_math(
  min_num,      # minimum limit for operand
  max_num,      # maximum limit for operand
  prompts,      # number of questions to generate
  difficulty,   # maximum distance between choices
  ratio = 0.5   # ratio of left / right answers
):
  operands_a_l = [] # text left           --> column 1
  operands_a_r= [] # text right          --> column 3
  operations_a = []

  operands_b_l = [] # text left           --> column 1
  operands_b_r= [] # text right          --> column 3
  operations_b = []

  solutions = []                          # correct answers     --> column 5
  operations = ["*", "-", "+"]            # removed division to remove fractional equations

  # generate answers
  for i in range(0, prompts):
    difference = difficulty + 100
    prompt_a_l, prompt_a_r, prompt_b_l, prompt_b_r, operation_a, operation_b = 0, 0, 0, 0, 0, 0
    while (difference > difficulty) or (difference == 0):
      prompt_a_l = random.randint(min_num, max_num)
      prompt_a_r = random.randint(min_num, max_num)
      prompt_b_l = random.randint(min_num, max_num)
      prompt_b_r = random.randint(min_num, max_num)
      operation_a = random.choice(operations)
      operation_b = random.choice(operations)

      difference = abs(get_solution(prompt_a_l, prompt_a_r, operation_a) - get_solution(prompt_b_l, prompt_b_r, operation_b))
      print(prompt_a_l, operation_a, prompt_a_r, "?", prompt_b_l, operation_b, prompt_b_r, "-->", difference)
    
    solution = "1" if (get_solution(prompt_a_l, prompt_a_r, operation_a) > get_solution(prompt_b_l, prompt_b_r, operation_b)) else "2"

    weighted_coin = random.uniform(0, 1)
    if weighted_coin <= ratio:
      if (solution == "1"):
        operands_a_l.append(prompt_a_l)
        operands_a_r.append(prompt_a_r)
        operations_a.append(operation_a)
        operands_b_l.append(prompt_b_l)
        operands_b_r.append(prompt_b_r)
        operations_b.append(operation_b)
      else: # switch problems
        operands_a_l.append(prompt_b_l)
        operands_a_r.append(prompt_b_r)
        operations_a.append(operation_b)
        operands_b_l.append(prompt_a_l)
        operands_b_r.append(prompt_a_r)
        operations_b.append(operation_a)
      solutions.append("1") # answer = left
    else:
      if (solution == "2"):
        operands_a_l.append(prompt_a_l)
        operands_a_r.append(prompt_a_r)
        operations_a.append(operation_a)
        operands_b_l.append(prompt_b_l)
        operands_b_r.append(prompt_b_r)
        operations_b.append(operation_b)
      else: # switch problems
        operands_a_l.append(prompt_b_l)
        operands_a_r.append(prompt_b_r)
        operations_a.append(operation_b)
        operands_b_l.append(prompt_a_l)
        operands_b_r.append(prompt_a_r)
        operations_b.append(operation_a)
      solutions.append("2") # answer = left

  # export prompts with answers to csv
  df = pd.DataFrame({'operand_a_l': operands_a_l, 'operand_a_r': operands_a_r, 'operation_a': operations_a, 'operand_b_l': operands_b_l, 'operand_b_r': operands_b_r, 'operation_b': operations_b, 'solution': solutions}).astype(str)
  formatDataframe(df, "math-questions")

#################################################################
#                     TOWERS OF HANOI                           #
#################################################################

# determine if proposed hanoi orientation is valid/legal
def valid_hanoi(peg1, peg2, peg3, blocks):
  items = peg1 + peg2 + peg3
  return ((len(list(set(items))) == len(items)) and (len(list(set(items))) == blocks))

# generate all possible block orientations on a single peg
def get_orientations(blocks):
  orientations = [""] # no block on peg
  for i in range(0, blocks): # get all possible stacks
    orientation = ""
    for j in range(i, -1, -1): 
      orientation = orientation + str(j)
      orientations.append(orientation)
      for k in range(0, len(orientation)):
        alternative = orientation[0 : k : ] + orientation[k + 1 : :]
        orientations.append(alternative)
  orientations = list(set(orientations))
  orientations.sort(key=len) # last element is goal orientation
  return orientations

# generate tower of hanoi problems
def generate_hanoi(
  prompts, 
  moves_min, # minimum moves passed before interruption
  moves_max, # maximum moves passed before interruption
  blocks = 4
):
  start_peg1 = []
  start_peg2 = []
  start_peg3 = []

  goal_peg1 = []
  goal_peg2 = []
  goal_peg3 = []

  orientations = get_orientations(blocks)
  start_orientations = orientations[:-1]
  goal_orientation = orientations[-1]

  moves = []

  for i in range(0, prompts):
    moves.append(random.randint(moves_min, moves_max))
    
    valid = False
    peg_a, peg_b, peg_c = "", "", "" 
    while (not valid):
      peg_a = random.choice(start_orientations)
      peg_b = random.choice(start_orientations)
      peg_c = random.choice(start_orientations)
      valid = valid_hanoi(peg_a, peg_b, peg_c, blocks)
    print(peg_a, peg_b, peg_c, "--(goal)-->", goal_orientation, "--(interrupted at)-->", moves[-1])
    
    # valid hanoi start state found
    start_peg1.append(peg_a)
    start_peg2.append(peg_b)
    start_peg3.append(peg_c)

    # hanoi end state (always all blocks on peg3)
    goal_peg3.append(goal_orientation)
    goal_peg2.append("")
    goal_peg1.append("")

  # export prompts with answers to csv
  df = pd.DataFrame({'start_peg1': start_peg1, 'start_peg2': start_peg2, 'start_peg3': start_peg3, 'goal_peg1': goal_peg1, 'goal_peg2': goal_peg2, 'goal_peg3': goal_peg3, 'moves': moves})
  formatDataframe(df, "hanoi-questions")
  # df.to_csv("hanoi-questions.csv", index=False)
    
#################################################################
#                  NOISE INTERRUPTION TASK                      #
#################################################################

# generate noise interruptions tasks
def generate_noise(
  options = 5,        # number of sounds available
  prompts = 50,       # number of questions to generate
):
  questions = []      # sound sequences   --> column 1

  while len(questions) < prompts:
    question = random.randint(0, options - 1)
    questions.append(question)

  # export questions sequence to csv
  df = pd.DataFrame({'question': questions})
  df.to_csv("noise-questions.csv", index=False)

#################################################################
#                  SOCIAL INTERRUPTION TASK                      #
#################################################################

# generate social interruptions tasks
def generate_social(
  options = 2,        # number of sounds available
  prompts = 50,       # number of questions to generate
):
  questions = []      # sound sequences   --> column 1

  while len(questions) < prompts:
    question = random.randint(0, options - 1)
    questions.append(question)

  # export questions sequence to csv
  df = pd.DataFrame({'question': questions})
  df.to_csv("social-questions.csv", index=False)

def main():
  # EXAMPLE USES
  # generate_path(options = 6, prompts = 100, prompt_length = 4)
  # generate_stroop(options = ["red", "black", "yellow", "green", "blue"], prompts = 400, ratio = 0.5)
  # generate_math(min_num = 5, max_num = 10, prompts = 400, difficulty = 2, ratio = 0.5)
  # generate_hanoi(prompts = 100, moves_min = 2, moves_max = 5, blocks = 4)
  # generate_noise(options = 5, prompts = 500)
  # generate_social(options = 2, prompts = 500)
  # readPathStructure()

if __name__ == "__main__":
  main()