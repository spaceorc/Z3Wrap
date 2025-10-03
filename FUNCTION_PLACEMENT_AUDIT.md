================================================================================
FUNCTION PLACEMENT AUDIT
================================================================================

## FUNCTIONS IN WRONG FILES

  Z3_append_log:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.InteractionLogging.cs
    Source: z3_api_interaction_logging.txt

  Z3_apply_result_dec_ref:
    Currently in: NativeLibrary.ReferenceCountingExtra.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_apply_result_inc_ref:
    Currently in: NativeLibrary.ReferenceCountingExtra.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_ast_to_string:
    Currently in: NativeLibrary.Functions.cs
    Should be in: NativeLibrary.StringConversion.cs
    Source: z3_api_string_conversion.txt

  Z3_benchmark_to_smtlib_string:
    Currently in: NativeLibrary.Parsing.cs
    Should be in: NativeLibrary.StringConversion.cs
    Source: z3_api_string_conversion.txt

  Z3_close_log:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.InteractionLogging.cs
    Source: z3_api_interaction_logging.txt

  Z3_constructor_num_fields:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_del_config:
    Currently in: NativeLibrary.Context.cs
    Should be in: NativeLibrary.Configuration.cs
    Source: z3_api_create_configuration.txt

  Z3_del_constructor:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_del_constructor_list:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_disable_trace:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.Miscellaneous.cs
    Source: z3_api_miscellaneous.txt

  Z3_enable_trace:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.Miscellaneous.cs
    Source: z3_api_miscellaneous.txt

  Z3_finalize_memory:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.Miscellaneous.cs
    Source: z3_api_miscellaneous.txt

  Z3_func_decl_to_string:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.StringConversion.cs
    Source: z3_api_string_conversion.txt

  Z3_func_entry_dec_ref:
    Currently in: NativeLibrary.ReferenceCountingExtra.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_entry_get_arg:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_entry_get_num_args:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_entry_get_value:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_entry_inc_ref:
    Currently in: NativeLibrary.ReferenceCountingExtra.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_interp_add_entry:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_interp_dec_ref:
    Currently in: NativeLibrary.ReferenceCountingExtra.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_interp_get_arity:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_interp_get_else:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_interp_get_entry:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_interp_get_num_entries:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_interp_inc_ref:
    Currently in: NativeLibrary.ReferenceCountingExtra.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_func_interp_set_else:
    Currently in: NativeLibrary.FunctionInterpretations.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_get_algebraic_number_lower:
    Currently in: NativeLibrary.AlgebraicNumbers.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_algebraic_number_upper:
    Currently in: NativeLibrary.AlgebraicNumbers.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_app_arg:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_app_decl:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_app_num_args:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_arity:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_array_sort_domain:
    Currently in: NativeLibrary.Arrays.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_array_sort_range:
    Currently in: NativeLibrary.Arrays.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_ast_hash:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_ast_id:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_ast_kind:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_bool_value:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_bv_sort_size:
    Currently in: NativeLibrary.BitVectors.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_datatype_sort_constructor:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_datatype_sort_constructor_accessor:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_datatype_sort_num_constructors:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_datatype_sort_recognizer:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_ast_parameter:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_double_parameter:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_func_decl_parameter:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_int_parameter:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_kind:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_name:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_num_parameters:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_parameter_kind:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_rational_parameter:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_sort_parameter:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_decl_symbol_parameter:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_denominator:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_domain:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_domain_size:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_full_version:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.Miscellaneous.cs
    Source: z3_api_miscellaneous.txt

  Z3_get_numeral_binary_string:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_decimal_string:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_double:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_int:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_int64:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_rational_int64:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_small:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_string:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_uint:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numeral_uint64:
    Currently in: NativeLibrary.Numerals.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_numerator:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_pattern:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_pattern_num_terms:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_quantifier_body:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_quantifier_bound_name:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_quantifier_bound_sort:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_quantifier_num_bound:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_quantifier_num_patterns:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_quantifier_pattern_ast:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_range:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_relation_arity:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_relation_column:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_sort:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_sort_kind:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_sort_name:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_symbol_int:
    Currently in: NativeLibrary.Symbols.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_symbol_string:
    Currently in: NativeLibrary.Queries.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_tuple_sort_field_decl:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_tuple_sort_mk_decl:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_tuple_sort_num_fields:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_get_version:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.Miscellaneous.cs
    Source: z3_api_miscellaneous.txt

  Z3_global_param_get:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.GlobalParameters.cs
    Source: z3_api_global_parameters.txt

  Z3_global_param_reset_all:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.GlobalParameters.cs
    Source: z3_api_global_parameters.txt

  Z3_global_param_set:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.GlobalParameters.cs
    Source: z3_api_global_parameters.txt

  Z3_is_algebraic_number:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_app:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_as_array:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_is_char_sort:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.StringTheory.cs
    Source: z3_api_sequences_and_regular_expressions.txt

  Z3_is_eq_ast:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_eq_func_decl:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_eq_sort:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_ground:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_lambda:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_numeral_ast:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_quantifier_exists:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_quantifier_forall:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_re_sort:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.StringTheory.cs
    Source: z3_api_sequences_and_regular_expressions.txt

  Z3_is_recursive_datatype_sort:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_is_seq_sort:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.StringTheory.cs
    Source: z3_api_sequences_and_regular_expressions.txt

  Z3_is_string:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.StringTheory.cs
    Source: z3_api_sequences_and_regular_expressions.txt

  Z3_is_string_sort:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.StringTheory.cs
    Source: z3_api_sequences_and_regular_expressions.txt

  Z3_is_well_sorted:
    Currently in: NativeLibrary.Predicates.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_mk_abs:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_add:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_and:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_mk_app:
    Currently in: NativeLibrary.Functions.cs
    Should be in: NativeLibrary.ConstantsAndApplications.cs
    Source: z3_api_constants_and_applications.txt

  Z3_mk_array_ext:
    Currently in: NativeLibrary.Arrays.cs
    Should be in: NativeLibrary.Sets.cs
    Source: z3_api_sets.txt

  Z3_mk_array_sort:
    Currently in: NativeLibrary.Arrays.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_atleast:
    Currently in: NativeLibrary.Constraints.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_mk_atmost:
    Currently in: NativeLibrary.Constraints.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_mk_bool_sort:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_bv_numeral:
    Currently in: NativeLibrary.BitVectors.cs
    Should be in: NativeLibrary.Numerals.cs
    Source: z3_api_numerals.txt

  Z3_mk_bv_sort:
    Currently in: NativeLibrary.BitVectors.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_config:
    Currently in: NativeLibrary.Context.cs
    Should be in: NativeLibrary.Configuration.cs
    Source: z3_api_create_configuration.txt

  Z3_mk_const:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.ConstantsAndApplications.cs
    Source: z3_api_constants_and_applications.txt

  Z3_mk_constructor:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_constructor_list:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_datatype:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_datatype_sort:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_datatypes:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_distinct:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_mk_div:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_divides:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_enumeration_sort:
    Currently in: NativeLibrary.SpecialTheories.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_eq:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_mk_finite_domain_sort:
    Currently in: NativeLibrary.SpecialTheories.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_fresh_const:
    Currently in: NativeLibrary.SpecialTheories.cs
    Should be in: NativeLibrary.ConstantsAndApplications.cs
    Source: z3_api_constants_and_applications.txt

  Z3_mk_fresh_func_decl:
    Currently in: NativeLibrary.SpecialTheories.cs
    Should be in: NativeLibrary.ConstantsAndApplications.cs
    Source: z3_api_constants_and_applications.txt

  Z3_mk_func_decl:
    Currently in: NativeLibrary.Functions.cs
    Should be in: NativeLibrary.ConstantsAndApplications.cs
    Source: z3_api_constants_and_applications.txt

  Z3_mk_ge:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_gt:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_iff:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_mk_implies:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_mk_int2real:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_int_sort:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_is_int:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_ite:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_mk_le:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_list_sort:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_lt:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_mod:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_mul:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_not:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_mk_numeral:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.Numerals.cs
    Source: z3_api_numerals.txt

  Z3_mk_or:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_mk_pbeq:
    Currently in: NativeLibrary.Constraints.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_mk_pbge:
    Currently in: NativeLibrary.Constraints.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_mk_pble:
    Currently in: NativeLibrary.Constraints.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_mk_power:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_probe:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_mk_real2int:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_real_sort:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_rem:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_sbv_to_str:
    Currently in: NativeLibrary.BitVectors.cs
    Should be in: NativeLibrary.StringTheory.cs
    Source: z3_api_sequences_and_regular_expressions.txt

  Z3_mk_set_has_size:
    Currently in: NativeLibrary.Sets.cs
    Should be in: NativeLibrary.Arrays.cs
    Source: z3_api_arrays.txt

  Z3_mk_simple_solver:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_mk_simplifier:
    Currently in: NativeLibrary.Simplifiers.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_mk_solver:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_mk_solver_for_logic:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_mk_solver_from_tactic:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_mk_sub:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_tuple_sort:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_mk_ubv_to_str:
    Currently in: NativeLibrary.BitVectors.cs
    Should be in: NativeLibrary.StringTheory.cs
    Source: z3_api_sequences_and_regular_expressions.txt

  Z3_mk_unary_minus:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.IntegersAndReals.cs
    Source: z3_api_integers_and_reals.txt

  Z3_mk_xor:
    Currently in: NativeLibrary.Expressions.cs
    Should be in: NativeLibrary.PropositionalLogicAndEquality.cs
    Source: z3_api_propositional_logic_and_equality.txt

  Z3_model_dec_ref:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_eval:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_const_decl:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_const_interp:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_func_decl:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_func_interp:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_num_consts:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_num_funcs:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_num_sorts:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_sort:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_get_sort_universe:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_has_interp:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_inc_ref:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_model_to_string:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.StringConversion.cs
    Source: z3_api_string_conversion.txt

  Z3_model_translate:
    Currently in: NativeLibrary.Model.cs
    Should be in: NativeLibrary.Models.cs
    Source: z3_api_models.txt

  Z3_open_log:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.InteractionLogging.cs
    Source: z3_api_interaction_logging.txt

  Z3_param_descrs_dec_ref:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.ParameterDescriptions.cs
    Source: z3_api_parameter_descriptions.txt

  Z3_param_descrs_get_documentation:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.ParameterDescriptions.cs
    Source: z3_api_parameter_descriptions.txt

  Z3_param_descrs_get_kind:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.ParameterDescriptions.cs
    Source: z3_api_parameter_descriptions.txt

  Z3_param_descrs_get_name:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.ParameterDescriptions.cs
    Source: z3_api_parameter_descriptions.txt

  Z3_param_descrs_inc_ref:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.ParameterDescriptions.cs
    Source: z3_api_parameter_descriptions.txt

  Z3_param_descrs_size:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.ParameterDescriptions.cs
    Source: z3_api_parameter_descriptions.txt

  Z3_param_descrs_to_string:
    Currently in: NativeLibrary.Parameters.cs
    Should be in: NativeLibrary.ParameterDescriptions.cs
    Source: z3_api_parameter_descriptions.txt

  Z3_pattern_to_string:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.StringConversion.cs
    Source: z3_api_string_conversion.txt

  Z3_probe_and:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_apply:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_const:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_dec_ref:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_eq:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_ge:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_get_descr:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_gt:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_inc_ref:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_le:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_lt:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_not:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_probe_or:
    Currently in: NativeLibrary.Probes.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_query_constructor:
    Currently in: NativeLibrary.Datatypes.cs
    Should be in: NativeLibrary.Sorts.cs
    Source: z3_api_sorts.txt

  Z3_reset_memory:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.Miscellaneous.cs
    Source: z3_api_miscellaneous.txt

  Z3_set_ast_print_mode:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.StringConversion.cs
    Source: z3_api_string_conversion.txt

  Z3_set_error:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.ErrorHandling.cs
    Source: z3_api_error_handling.txt

  Z3_set_param_value:
    Currently in: NativeLibrary.Context.cs
    Should be in: NativeLibrary.Configuration.cs
    Source: z3_api_create_configuration.txt

  Z3_simplifier_and_then:
    Currently in: NativeLibrary.Simplifiers.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_simplifier_dec_ref:
    Currently in: NativeLibrary.Simplifiers.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_simplifier_get_descr:
    Currently in: NativeLibrary.Simplifiers.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_simplifier_get_help:
    Currently in: NativeLibrary.Simplifiers.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_simplifier_get_param_descrs:
    Currently in: NativeLibrary.Simplifiers.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_simplifier_inc_ref:
    Currently in: NativeLibrary.Simplifiers.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_simplifier_using_params:
    Currently in: NativeLibrary.Simplifiers.cs
    Should be in: NativeLibrary.Tactics.cs
    Source: z3_api_tactics_simplifiers_and_probes.txt

  Z3_simplify:
    Currently in: NativeLibrary.Simplify.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_simplify_ex:
    Currently in: NativeLibrary.Simplify.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_simplify_get_help:
    Currently in: NativeLibrary.Simplify.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_simplify_get_param_descrs:
    Currently in: NativeLibrary.Simplify.cs
    Should be in: NativeLibrary.Accessors.cs
    Source: z3_api_accessors.txt

  Z3_solver_assert:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_assert_and_track:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_check:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_check_assumptions:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_cube:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_dec_ref:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_from_file:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_from_string:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_assertions:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_consequences:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_help:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_model:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_num_scopes:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_param_descrs:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_proof:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_reason_unknown:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_statistics:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_get_unsat_core:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_inc_ref:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_interrupt:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_pop:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_push:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_reset:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_set_params:
    Currently in: NativeLibrary.Solver.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_to_dimacs_string:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_to_string:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_solver_translate:
    Currently in: NativeLibrary.SolverExtensions.cs
    Should be in: NativeLibrary.Solvers.cs
    Source: z3_api_solvers.txt

  Z3_sort_to_string:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.StringConversion.cs
    Source: z3_api_string_conversion.txt

  Z3_stats_dec_ref:
    Currently in: NativeLibrary.ReferenceCountingExtra.cs
    Should be in: NativeLibrary.Statistics.cs
    Source: z3_api_statistics.txt

  Z3_stats_inc_ref:
    Currently in: NativeLibrary.ReferenceCountingExtra.cs
    Should be in: NativeLibrary.Statistics.cs
    Source: z3_api_statistics.txt

  Z3_substitute:
    Currently in: NativeLibrary.Substitution.cs
    Should be in: NativeLibrary.Modifiers.cs
    Source: z3_api_modifiers.txt

  Z3_substitute_funs:
    Currently in: NativeLibrary.Substitution.cs
    Should be in: NativeLibrary.Modifiers.cs
    Source: z3_api_modifiers.txt

  Z3_substitute_vars:
    Currently in: NativeLibrary.Substitution.cs
    Should be in: NativeLibrary.Modifiers.cs
    Source: z3_api_modifiers.txt

  Z3_toggle_warning_messages:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.InteractionLogging.cs
    Source: z3_api_interaction_logging.txt

  Z3_translate:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.Modifiers.cs
    Source: z3_api_modifiers.txt

  Z3_update_term:
    Currently in: NativeLibrary.Utilities.cs
    Should be in: NativeLibrary.Modifiers.cs
    Source: z3_api_modifiers.txt

## MISSING FUNCTIONS

Total missing functions: 90

z3_api_accessors.txt -> NativeLibrary.Accessors.cs: Missing 19 functions
  - Z3_app_to_ast
  - Z3_datatype_update_field
  - Z3_func_decl_to_ast
  - Z3_get_array_arity
  - Z3_get_array_sort_domain_n
  - Z3_get_depth
  - Z3_get_finite_domain_sort_size
  - Z3_get_func_decl_id
  - Z3_get_index_value
  - Z3_get_quantifier_id
  ... and 9 more

z3_api_arrays.txt -> NativeLibrary.Arrays.cs: Missing 3 functions
  - Z3_mk_map
  - Z3_mk_select_n
  - Z3_mk_store_n

z3_api_bit_vectors.txt -> NativeLibrary.BitVectors.cs: Missing 1 functions
  - Z3_mk_bit2bool

z3_api_constants_and_applications.txt -> NativeLibrary.ConstantsAndApplications.cs: Missing 2 functions
  - Z3_add_rec_def
  - Z3_mk_rec_func_decl

z3_api_context_and_ast_reference_counting.txt -> NativeLibrary.Context.cs: Missing 4 functions
  - Z3_enable_concurrent_dec_ref
  - Z3_get_global_param_descrs
  - Z3_interrupt
  - Z3_mk_context

z3_api_models.txt -> NativeLibrary.Models.cs: Missing 4 functions
  - Z3_add_const_interp
  - Z3_add_func_interp
  - Z3_get_as_array_func_decl
  - Z3_mk_model

z3_api_numerals.txt -> NativeLibrary.Numerals.cs: Missing 6 functions
  - Z3_mk_int
  - Z3_mk_int64
  - Z3_mk_real
  - Z3_mk_real_int64
  - Z3_mk_unsigned_int
  - Z3_mk_unsigned_int64

z3_api_quantifiers.txt -> NativeLibrary.Quantifiers.cs: Missing 6 functions
  - Z3_mk_exists
  - Z3_mk_forall
  - Z3_mk_quantifier
  - Z3_mk_quantifier_const
  - Z3_mk_quantifier_const_ex
  - Z3_mk_quantifier_ex

z3_api_sequences_and_regular_expressions.txt -> NativeLibrary.StringTheory.cs: Missing 1 functions
  - Z3_get_lstring

z3_api_solvers.txt -> NativeLibrary.Solvers.cs: Missing 25 functions
  - Z3_get_implied_equalities
  - Z3_solver_congruence_explain
  - Z3_solver_congruence_next
  - Z3_solver_congruence_root
  - Z3_solver_get_levels
  - Z3_solver_get_non_units
  - Z3_solver_get_trail
  - Z3_solver_get_units
  - Z3_solver_import_model_converter
  - Z3_solver_next_split
  ... and 15 more

z3_api_sorts.txt -> NativeLibrary.Sorts.cs: Missing 3 functions
  - Z3_mk_array_sort_n
  - Z3_mk_type_variable
  - Z3_mk_uninterpreted_sort

z3_api_special_relations.txt -> NativeLibrary.SpecialTheories.cs: Missing 4 functions
  - Z3_mk_linear_order
  - Z3_mk_partial_order
  - Z3_mk_piecewise_linear_order
  - Z3_mk_tree_order

z3_api_statistics.txt -> NativeLibrary.Statistics.cs: Missing 1 functions
  - Z3_get_estimated_alloc_size

z3_api_symbols.txt -> NativeLibrary.Symbols.cs: Missing 1 functions
  - Z3_mk_int_symbol

z3_api_tactics_simplifiers_and_probes.txt -> NativeLibrary.Tactics.cs: Missing 10 functions
  - Z3_apply_result_get_num_subgoals
  - Z3_apply_result_get_subgoal
  - Z3_apply_result_to_string
  - Z3_get_num_probes
  - Z3_get_num_simplifiers
  - Z3_get_num_tactics
  - Z3_get_probe_name
  - Z3_get_simplifier_name
  - Z3_get_tactic_name
  - Z3_solver_add_simplifier

## DUPLICATE FUNCTIONS

  Z3_mk_sbv_to_str:
    - NativeLibrary.BitVectors.cs
    - NativeLibrary.StringTheory.cs

  Z3_mk_ubv_to_str:
    - NativeLibrary.BitVectors.cs
    - NativeLibrary.StringTheory.cs

================================================================================
SUMMARY STATISTICS
================================================================================

Functions in c_headers/*.txt: 556
Functions in NativeLibrary*.cs: 600
Misplaced functions: 263
Missing functions: 90
Duplicate functions: 2

